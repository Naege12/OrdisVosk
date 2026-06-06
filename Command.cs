using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdisVosk
{
    internal class Command
    {
        private string _command;
        private string _description;
        private string _filepath;
        private bool _isSystemCommand = false;

        public Command(string command, string description, string filepath)
        {
            _command = command;
            _description = description;
            _filepath = filepath;
        }

        public string getCommandText { get => _command; }
        public string getDescription { get => _description; }
        public string getFilepath { get => _filepath; }
        public bool getIsSystemCommand { get => _isSystemCommand; }
        public void setCommand(string command)
        {
            _command = command;
        }
        public void setDescription(string description)
        {
            _description = description;
        }
        public void setFilepath(string filepath)
        {
            _filepath = filepath;
        }
    }
}
