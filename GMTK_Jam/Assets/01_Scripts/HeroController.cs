
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : EntityController {

    private void Update() {
        animator.SetFloat("XVelocity", velocity.x);
        animator.SetFloat("YVelocity", velocity.y);
    }

}