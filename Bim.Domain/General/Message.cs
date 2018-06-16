using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.General
{
    public class Message
    {


        public static int IdGenerator { get; set; } = 0;
        public int Id { get; set; }
        public string Problem { get; set; }
        public string Solution { get; set; }
        public MessageType MessageType { get; set; }
        public Message(MessageType messageType, string problem, string solution)
        {
            //get Id
            Id = IdGenerator;
            //
            MessageType = messageType;
            Problem = problem;
            Solution = solution;

            //increase the Count
            IdGenerator++;
        }

    }
}
