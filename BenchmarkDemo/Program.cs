
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            DoSequentialWork_ForEach().Wait();
            DoParallelWork_WhenAll().Wait();

            sw.Stop();
            Console.WriteLine($"Time Lapsed {sw.ElapsedMilliseconds}ms");
            Console.ReadLine();


        }

        // sequentially invoke the methods, waiting for each to finish before starting the next
        async static Task DoSequentialWork_ForEach()
        {
            var vms = new List<ViewModel>();
            foreach (var dto in Getdtos())
            {
                var vm = await CreateViewModel(dto);
                vms.Add(vm);
            }
        }



        async static Task<IEnumerable<ViewModel>> DoParallelWork_WhenAll()
        {
            var vms = new List<Task<ViewModel>>();
            foreach (var dto in Getdtos())
            {
                var vm =  CreateViewModel(dto);
                 vms.Add(vm);
            }

            return await Task.WhenAll(vms);
        }


        async static Task<ViewModel> CreateViewModel(Dto dto)
        {
            Console.WriteLine($"Creating VM from Dto {dto.OperationIndex}");
            await Task.Delay(200);
            var vm = new ViewModel { OperationIndex = dto.OperationIndex };
            Console.WriteLine($"Created VM with Index {vm.OperationIndex}");
            return vm;
        }

        private static List<Dto> Getdtos()
        {
            var results = new List<Dto>();
            for (int i = 0; i < 100; i++)
            {
                results.Add(new Dto { OperationIndex = i });

            }
            return results;
        }
    }

    class ViewModel
    {
        public int OperationIndex { get; set; }

    }

    class Dto
    {
        public int OperationIndex { get; set; }
    }


}
