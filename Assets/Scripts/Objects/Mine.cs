using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
    public class Mine : SpaceObject
    {
        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Init()
        {
            base.Init();
        }

        #endregion

        protected override void OnDamageApplied(IObject target)
        {
            base.OnDamageApplied(target);
            SelfDestroy();
        }

    }
}