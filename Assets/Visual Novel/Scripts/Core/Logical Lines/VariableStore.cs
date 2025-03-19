using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class VariableStore {
    private const string DEFAULT_DATABASE_NAME = "Default";
    private const char  DATABASE_VARIABLE_RELATIONAL_ID = '.';

    public class Database {
        public string name;
        public Dictionary<string, Variable> variables = new();

        public Database(string name) {
            this.name = name;
            variables = new();
        }
    }

    public abstract class Variable {
        public abstract object Get();
        public abstract void Set(object value);
    }

    public class Variable<T> : Variable
    {
        private T value;

        private Func<T> getter;
        private Action<T> setter;

        public Variable(T defaultValue = default, Func<T> getter = null, Action<T> setter = null) {
            value = defaultValue;
            
            this.getter = getter == null
                ? () => value // If getter is null, make a new getter
                : getter;     // Otherwise use the getter passed in
            
            this.setter = setter == null
                ? (newValue) => value = newValue // If getter is null, make a new getter
                : setter;                        // Otherwise use the setter passed in
        }

        public override object Get() => getter();

        public override void Set(object newValue) => setter((T)newValue);
    }

    private static Dictionary<string, Database> databases = new() {
        { DEFAULT_DATABASE_NAME, new(DEFAULT_DATABASE_NAME) }
    };
    private static Database defaultDatabase => databases[DEFAULT_DATABASE_NAME];

    public static bool CreateDatabase(string name) {
        if (!databases.ContainsKey(name)) {
            databases[name] = new(name);
            return true;
        }
        return false; // Did not create database, so return false
    }

    public static Database GetDatabase(string name) {
        // If a name is not provided, return the default database
        if (name == string.Empty) {
            return defaultDatabase;
        }

        if (!databases.ContainsKey(name)) {
            // If the database doesn't exist, create it
            CreateDatabase(name);
        }

        return databases[name];
    }

    public static bool CreateVariable<T>(string name, T defaultValue, Func<T> getter = null, Action<T> setter = null) {
        (string[] parts, Database db, string variableName) = ExtractInfo(name);
        
        if (db.variables.ContainsKey(variableName)) {
            return false; // Variable already exists, no need to create it
        }

        db.variables[variableName] = new Variable<T>(defaultValue, getter, setter);

        return true;
    }

    public static bool TryGetValue(string name, out object variable) {
        (string[] parts, Database db, string variableName) = ExtractInfo(name);
        
        if (!db.variables.ContainsKey(variableName)) {
            // Variable does not exist
            variable = null;
            return false;
        }

        variable = db.variables[variableName].Get();
        return true;
    }

    public static bool TrySetValue<T>(string name, T value) {
        (string[] parts, Database db, string variableName) = ExtractInfo(name);
        
        if (!db.variables.ContainsKey(variableName)) {
            // Variable does not exist
            return false;
        }

        db.variables[variableName].Set(value);
        return true;
    }

    private static (string[], Database, string) ExtractInfo(string name) {
        string[] parts = name.Split(DATABASE_VARIABLE_RELATIONAL_ID);
        Database db = parts.Length > 1 ? GetDatabase(parts[0]) : defaultDatabase;
        string variableName = parts.Length > 1 ? parts[1] : parts[0];
        return (parts, db, variableName);
    }

    public static void RemoveVariable(string name) {
        (string[] parts, Database db, string variableName) = ExtractInfo(name);

        if (db.variables.ContainsKey(variableName)) {
            db.variables.Remove(variableName);
        }
    }

    public static void RemoveAllVariables() {
        databases.Clear();
        databases[DEFAULT_DATABASE_NAME] = new(DEFAULT_DATABASE_NAME);
    }

    public static void PrintAllDatabases() {
        foreach(KeyValuePair<string, Database> dbEntry in databases) {
            Debug.Log($"Database: '<color=$FFB145>{dbEntry.Key}</color>'");
        }
    }

    public static void PrintAllVariables(Database database = null) {
        if (database != null) {
            PrintAllDatabaseVariables(database);
            return;
        }

        foreach (var dbEntry in databases) {
            PrintAllDatabaseVariables(dbEntry.Value);
        }
    }

    private static void PrintAllDatabaseVariables(Database database) {
        StringBuilder sb = new();

        sb.AppendLine($"Database: <color=#F38544>{database.name}</color>");

        foreach (KeyValuePair<string, Variable> variablePair in database.variables) {
            string variableName = variablePair.Key;
            object variableValue = variablePair.Value.Get();
            sb.AppendLine($"\t<color=#FFB145>Variable [{variableName}]</color> = <color=#FFD22D>{variableValue}</color>");
        }

        Debug.Log(sb.ToString());
    }

}
