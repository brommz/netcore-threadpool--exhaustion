using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThreadsTester.Controllers;
using Microsoft.AspNetCore.Mvc;
using ThreadsTester.Services;
using System.Threading;
using ThreadsTester.Models;

namespace ThreadsTester.Controllers
{
    public class TestsRunnerController : BaseController
    {
        private readonly AsyncService service;

        public TestsRunnerController(AsyncService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return Ok();
        }

        /// <summary>
        /// Example
        /// e.g. siege -c 200 -t1M https://localhost:5001/test/uses-too-many-threadpool-threads-v1
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/uses-too-many-threadpool-threads-v1")]
        public IActionResult SyncOverAsyncResultV1()
        {
            string val = service.DoAsyncOperationWell().Result;

            return Ok(val);
        }

                /// <summary>
        /// Example
        /// e.g. siege -c 200 -t1M https://localhost:5001/test/uses-too-many-threadpool-threads-vtest
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/uses-too-many-threadpool-threads-vtest")]
        public IActionResult SyncOverAsyncResultVTest()
        {
            string val = service.DoAsyncOperationWell().GetAwaiter().GetResult();

            return Ok(val);
        }

        /// <summary>
        /// Example
        /// e.g. siege -c 200 -t1M https://localhost:5001/test/uses-too-many-threadpool-threads-v2
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/uses-too-many-threadpool-threads-v2")]
        public IActionResult SyncOverAsyncResultV2()
        {
            string val = Task.Run(() => service.DoAsyncOperationWell()).GetAwaiter().GetResult();

            return Ok(val);
        }

        /// <summary>
        /// Example
        /// e.g. siege -c 200 -t1M https://localhost:5001/test/uses-too-many-threadpool-threads-v3
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/uses-too-many-threadpool-threads-v3")]
        public IActionResult SyncOverAsyncResultV3()
        {
            string val = Task.Run(() => service.DoAsyncOperationWell().GetAwaiter().GetResult()).GetAwaiter().GetResult();

            return Ok(val);
        }

        /// <summary>
        /// Example
        /// e.g. siege -c 200 -t1M https://localhost:5001/test/uses-too-many-threadpool-threads-v4
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/uses-too-many-threadpool-threads-v4")]
        public IActionResult SyncOverAsyncResultV4()
        {

            var task = service.DoAsyncOperationWell();
            task.Wait();
            string val = task.GetAwaiter().GetResult();

            return Ok(val);
        }


        /// <summary>
        /// Example
        /// siege -c 200 -t1M https://localhost:5001/test/async-all
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/async-all")]
        public async Task<IActionResult> SyncOverAsyncResult()
        {
            var result = await service.DoAsyncOperationWell();

            return Ok(result);
        }

        /// <summary>
        /// Example
        /// siege -c 200 -t1M https://localhost:5001/test/uses-too-many-threadpool-threads-fixed
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/uses-too-many-threadpool-threads-fixed")]
        public IActionResult SyncOverAsyncResultFixedByAsyncHelper()
        {
            var result = AsyncHelper.RunSync<string>(service.DoAsyncOperationWell);
            return Ok(result);
        }

        /// <summary>
        /// Example
        /// siege -c 200 -t1M https://localhost:5001/test/uses-too-many-threadpool-threads-fixed-2
        /// </summary>
        /// <returns></returns>
        [HttpGet("test/uses-too-many-threadpool-threads-fixed-2")]
        public IActionResult SyncOverAsyncResultFixed2()
        {
            string result = null;
            var task = new Task(async () =>
                {
                    result = await service.DoAsyncOperationWell();
                });

            task.RunSynchronously();
            return Ok(result);
        }
    }
}
