using ThreadsTester.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsTester.Services
{
    public class AsyncService
    {
        public async Task<string> DoAsyncOperationWell()
        {
            var random = new Random();

            await Task.Delay(1000);

            return Guid.NewGuid().ToString();
        }
    }
}