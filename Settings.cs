using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdisVosk
{
    internal class Settings
    {
        public void SettingsMenu()
        {
            Console.WriteLine("Настройки пока не реализованы");
            Console.WriteLine("Нажми 1 для редактирования команд");
            Console.WriteLine("А также добавить свои команды");
            Console.ReadLine();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.D1)
                    {
                        EditCommand();
                    }
                }
            }
        }

        private void EditCommand()
        {
            Console.WriteLine("Редактирование команды");
            Console.WriteLine("Здесь будет возможность изменить команду, ее описание и путь к файлу");
            Console.ReadLine();
        }


    }
}
