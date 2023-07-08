
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : EntityController {

    private void Update() {
        animator.SetFloat("XVelocity", velocity.x);
    }

}