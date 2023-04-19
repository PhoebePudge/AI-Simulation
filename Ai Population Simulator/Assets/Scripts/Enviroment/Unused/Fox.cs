using System.Collections;
using UnityEngine;

//public class Fox : Animal {
//    private Animator anim;


//    /*public override void Init(Vector3 InitPosition) {
//        femaleColour = new Color(189 / 255f, 141 / 255f, 57 / 255f);
//        maleColour =  new Color(204f / 255f, 132f / 255f, 57f / 255f);

//        wanderRadius = 10;
//        speed = 1.5f;

//        base.Init(InitPosition);
//        transform.Translate(new Vector3(0, -0.5f, 0));
//        anim = gameObject.AddComponent<Animator>();
//        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("FoxController");
//        foodTarget = "Bunny";
//        BoxCollider bc = gameObject.GetComponent<BoxCollider>();
//        bc.center = new Vector3(0, 0.34f, 0.06f);
//        bc.size = new Vector3(0.47f, 0.55f, 0.78f);
//    }*/
//    protected override void AnimateMove() {
//        base.AnimateMove();

//        //make more effecient later
//        if (animatingMovement) {
//            anim.SetBool("Running", true);
//        } else {
//            anim.SetBool("Running", false);
//        }

//    }
//}
