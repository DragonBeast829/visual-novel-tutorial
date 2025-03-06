using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE {
    public class ConversationQueue {
        private Queue<Conversation> conversationQueue = new();
        public Conversation top => conversationQueue.Peek();

        public void Enqueue(Conversation conversation) => conversationQueue.Enqueue(conversation);

        public void EnqueuePriority(Conversation conversation) {
            Queue<Conversation> queue = new();
            queue.Enqueue(conversation);
            
            while (conversationQueue.Count > 0) {
                queue.Enqueue(conversationQueue.Dequeue());
            }

            conversationQueue = queue;
        }

        public void Dequeue() {
            if (conversationQueue.Count > 0) {
                conversationQueue.Dequeue();
            }
        }

        public bool IsEmpty() => conversationQueue.Count == 0;
    }
}
