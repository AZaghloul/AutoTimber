using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.General
{
    public class State
    {
        private StateResult currentState;

        public IfModel IfModel { get; set; }
        public bool IsUnitcompatible { get; set; }
        public bool IsVersionCombatible { get; set; }
        public bool HasBuilding { get; set; }
        public bool HasStories { get; set; }
        public bool HasWalls { get; set; }
        public bool HasExternalWalls { get; set; }
        public bool HasInternalWalls { get; set; }
        public bool HasSimpleLayout { get; set; }
        public List<Message> Messages { get; set; }


        public StateResult CurrentState
        {
            get { CheckState(); return currentState; }
            set => currentState = value;
        }

        public State()
        {
            Messages = new List<Message>();

        }


        private StateResult CheckState()
        {
            //Errors.Clear();
            //Warrnings.Clear();
            //Passed.Clear();\

            //default state optimistic

            if (!IsUnitcompatible && !IsVersionCombatible)
            {
                return CurrentState = StateResult.Failed;
            }
            if (!HasBuilding && !HasStories)
            {
                return CurrentState = StateResult.NeedFix;
            }

            if (!HasInternalWalls)
            {
                return CurrentState = StateResult.NeedFix;
            }

            if (!HasExternalWalls)
            {
                return CurrentState = StateResult.NeedFix;
            }

            if (!HasSimpleLayout)
            {
                return CurrentState = StateResult.NeedFix;
            }
            return CurrentState;
        }

        public void Warrnings(string problem, string solution)
        {
            Messages.Add(new Message(MessageType.Warrning, problem, solution));
        }
        public void Errors(MessageType type, string problem, string solution)
        {
            Messages.Add(new Message(MessageType.Error, problem, solution));
        }
        public void Passed(string problem, string solution)
        {
            Messages.Add(new Message(MessageType.Passed, problem, solution));
        }

    }
}
