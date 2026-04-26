using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProcessing
{
    internal interface IStreamProcessingOperation<InputType, OutputType>
    {
        public OutputType HandleMessage(IotMessage<InputType> message);
    }
}
