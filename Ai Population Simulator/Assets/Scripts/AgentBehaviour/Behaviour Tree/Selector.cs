using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree {
    public class Selector : Node {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        //Evaulate each child, end the first time we get a successful or running node
        public override NodeState Evaluate() {
            foreach (Node node in children) {
                switch (node.Evaluate()) {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

    }
}