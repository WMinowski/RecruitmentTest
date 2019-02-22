// Unused usings removed
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TodoApi.Models;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the 
        //container.
        public void ConfigureServices(IServiceCollection services)
        {
            ShellExecute(IntPtr.Zero, "open", "sqllocaldb.exe", "start MSSQLLocalDB", null, 1);
            string connection;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "sqllocaldb.exe";
                p.StartInfo.Arguments = "info MSSQLLocalDB";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();

                string[] cmdOutput = p.StandardOutput.ReadToEnd().Split(":");
                p.WaitForExit();
                connection = "Server=np:" + cmdOutput[cmdOutput.Length - 1] + "; Integrated Security=true; AttachDbFileName = " + AppDomain.CurrentDomain.BaseDirectory + "mydb.mdf;";


            }
                services.AddDbContext<TodoContext>(opt =>
                opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            StaticService.GetData(connection);
            services.Add(new ServiceDescriptor(typeof(CustomerService), new CustomerService()));
            services.Add(new ServiceDescriptor(typeof(CityService), new CityService()));
            services.Add(new ServiceDescriptor(typeof(PlaceService), new PlaceService()));
            services.Add(new ServiceDescriptor(typeof(CustomerPlaceService), new CustomerPlaceService()));
            //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP 
        //request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
