using Data;
using Enums;
using InteractableObjects.Characters;
using Interfaces;
using Managers;
using UnityEngine;

namespace Factory
{
    public class CharStateFactory
    {
        public CharStateFactory(DialogSystem dialogSystem)
        {
            DialogTransfer.dialogSystem ??= dialogSystem;
        }

        public ICharState CreateState(CharacterFSM fsm, CharStateData stateData,
            Transform transform, ICharacterService characterService)

        {
            ICharState resultState = null;
            switch (stateData.state)
            {
                case CharStates.StayAndTalk:
                    resultState = new StayAndTalkState(fsm);
                    break;
                
                case CharStates.StayAndChoiceTalk:
                    resultState = new StayAndChoiceTalk(fsm);
                    break;
                
                case CharStates.Recieve:
                    resultState = new RecieveState(fsm);
                    break;
                
                case CharStates.Give:
                    resultState = new GiverState(fsm);
                    break;
                
                case CharStates.Wait:
                    resultState = new WaitState(fsm);
                    break;
                
            }

            if (resultState != null)
            {
                resultState.SetStateBasics(
                    transform,
                    characterService);
                resultState.Set(stateData);
            }
            
            return resultState;
        }
    }
}