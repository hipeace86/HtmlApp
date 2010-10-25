using System;
using System.Collections.Generic;
using System.Text;

namespace Jfw.HtmlKit
{
    public abstract class AbstractHtml
    {
        /// <summary>
        /// 
        /// </summary>
        protected Task _task;
        internal Queue<string> paramsQueue;
        internal int Total = 0;
        public AbstractHtml(Task task)
        {
            _task = task;
            paramsQueue = new Queue<string>(10);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            if (_task.Type == TaskType.DataBase)
            {
                List<string> param = GetId();
                foreach (string s in param)
                {
                    paramsQueue.Enqueue(s);
                }
            }
            else if (_task.Type == TaskType.Customer)
            {
                while (_task.Current <= _task.End && _task.Status == TaskStatus.Run)
                {
                    paramsQueue.Enqueue(Convert.ToString(_task.Current * _task.Step));
                    _task.Current++;
                }
            }
            CreateHtmlThread();
        }
        /// <summary>
        /// 
        /// </summary>
        abstract public void Start();
        /// <summary>
        /// 
        /// </summary>
        abstract public void Stop();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        abstract public List<String> GetId();
        /// <summary>
        /// 
        /// </summary>
        abstract public void CreateHtmlThread();
    }

}
