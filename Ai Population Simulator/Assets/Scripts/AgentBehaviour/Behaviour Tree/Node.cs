using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base of a tutorial for a behaviour tree
//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
namespace BehaviorTree {
    //Defined our states
    public enum NodeState {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node {
        //Variables
        protected NodeState state;
        public Node parent;
        protected List<Node> children = new List<Node>();

        //Initilise
        public Node() {
            parent = null;
        }

        //Initilise with children
        public Node(List<Node> children) {
            foreach (Node child in children) {
                Attach(child);
            }
        }
        //To string conversion
        public override string ToString() {
            string output = this.GetType().ToString() + "\n";

            foreach (var item in children) {
                output += item.ToString();
            }

            return output;
        }
        //Attach our node to another node
        private void Attach(Node node) {
            node.parent = this;
            children.Add(node);

        }
        //We override this in each node script we write
        public virtual NodeState Evaluate() => NodeState.FAILURE;


        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        //Set data
        public void SetData(string key, object value) {
            dataContext[key] = value;
        }

        //Get our data
        public object GetData(string key) {
            object value = null;
            if (dataContext.TryGetValue(key, out value)) {
                return value;
            }

            Node node = parent;
            while (node != null) {
                value = node.GetData(key);
                if (value != null) {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }

        //Clear our data
        public bool ClearData(string key) {
            if (dataContext.ContainsKey(key)) {
                dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null) {
                bool cleared = node.ClearData(key);
                if (cleared) {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }
}
