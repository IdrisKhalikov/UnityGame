using System;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using GameController;

namespace Stamina
{
    public class StaminaBarUpdater : MonoBehaviour, MMEventListener<StaminaUpdateEvent>, MMEventListener<TopDownEngineEvent>
    {
        [SerializeField]
        [Tooltip("if this is false, the player character will be set as target automatically")]
        private bool UseCustomTarget;
        [MMCondition(nameof(UseCustomTarget), true)]
        [SerializeField]
        private GameObject Target;

        private MMProgressBar _bar;

        private void Awake()
        {
            _bar = GetComponent<MMProgressBar>();
            this.MMEventStartListening<TopDownEngineEvent>();
            Target = GameController.GameController.PlayerCharacter;
        }

        public void OnMMEvent(StaminaUpdateEvent staminaUpdateEvent)
        {
            if (staminaUpdateEvent.Target != Target) return;
            _bar.UpdateBar(staminaUpdateEvent.Stamina, 0, staminaUpdateEvent.MaxStamina);
        }

        public void OnMMEvent(TopDownEngineEvent topDownEngineEvent)
        {
            if (GameController.GameController.PlayerCharacter is not null && Target != GameController.GameController.PlayerCharacter)
                Target = GameController.GameController.PlayerCharacter;
        }

        public void Update()
        {
            if (GameController.GameController.PlayerCharacter is not null && Target != GameController.GameController.PlayerCharacter)
                Target = GameController.GameController.PlayerCharacter;
        }

        private void OnEnable()
        {
            this.MMEventStartListening<StaminaUpdateEvent>();
        }
    
        private void OnDisable()
        {
            this.MMEventStopListening<StaminaUpdateEvent>();
        }
    }
}