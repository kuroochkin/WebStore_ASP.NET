namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate Next;
        public TestMiddleware(RequestDelegate Next)
        {
            this.Next = Next;
        }

        public async Task Invoke(HttpContext Context)
        {
            // Обработка информации из Context.Request

            var processing_task = Next(Context); // далее здесь работает оставшаяся часть конвейера

            // Выполнить какие-то действия параллельно асинхронно с другйо частью конвейера

            await processing_task;
 

            // Дообработка данных в Context.Request
        }

    }
}
