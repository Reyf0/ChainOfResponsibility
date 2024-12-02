using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfResponsibility
{
    public class Request
    {
        public string EventType { get; set; }
        public string Data { get; set; }

        public Request(string eventType, string data)
        {
            EventType = eventType;
            Data = data;
        }
    }

    public abstract class Handler
    {
        protected Handler NextHandler { get; set; }

        public void SetNext(Handler handler)
        {
            NextHandler = handler;
        }

        public abstract void Handle(Request request);
    }

    public class ErrorHandler : Handler
    {
        public override void Handle(Request request)
        {
            if (request.EventType == "Error")
            {
                Console.WriteLine($"ErrorHandler: Обработка ошибки: {request.Data}");
            }
            else if (NextHandler != null)
            {
                NextHandler.Handle(request);
            }
        }
    }

    public class WarningHandler : Handler
    {
        public override void Handle(Request request)
        {
            if (request.EventType == "Warning")
            {
                Console.WriteLine($"WarningHandler: Обработка предупреждения: {request.Data}");
            }
            else if (NextHandler != null)
            {
                NextHandler.Handle(request);
            }
        }
    }


    public class InfoHandler : Handler
    {
        public override void Handle(Request request)
        {
            if (request.EventType == "Info")
            {
                Console.WriteLine($"InfoHandler: Обработка информационного сообщения: {request.Data}");
            }
            else if (NextHandler != null)
            {
                NextHandler.Handle(request);
            }
        }
    }


    internal class Program
    {
        public static void Main(string[] args)
        {
            // Создание обработчиков
            var errorHandler = new ErrorHandler();
            var warningHandler = new WarningHandler();
            var infoHandler = new InfoHandler();

            // Формирование цепочки
            errorHandler.SetNext(warningHandler);
            warningHandler.SetNext(infoHandler);

            // Создание запросов
            var errorRequest = new Request("Error", "Критическая ошибка!");
            var warningRequest = new Request("Warning", "Возможная проблема.");
            var infoRequest = new Request("Info", "Система работает корректно.");
            var unknownRequest = new Request("Unknown", "Неизвестное событие.");


            // Отправка запросов по цепочке
            errorHandler.Handle(errorRequest);
            errorHandler.Handle(warningRequest);
            errorHandler.Handle(infoRequest);
            errorHandler.Handle(unknownRequest); // Проверка на обработку неизвестного типа
        }
    }

}
