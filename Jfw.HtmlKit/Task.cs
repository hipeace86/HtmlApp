using System;
using System.Collections.Generic;
using System.Text;

namespace Jfw.HtmlKit
{
    /// <summary>
    /// 
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Task构造
        /// </summary>
        public Task()
        { }
        private string fname = string.Empty;
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name
        {
            get
            {
                return fname;
            }
            set
            {
                fname = value;
            }
        }
        private int istart = 0;
        /// <summary>
        /// 开始位置
        /// </summary>
        public int Start
        {
            get
            {
                return istart;
            }
            set
            {
                istart = value;
            }
        }
        private int iend = 1;
        /// <summary>
        /// 结束位置
        /// </summary>
        public int End
        {
            get
            {
                return iend;
            }
            set
            {
                iend = value;
            }
        }
        private int istep = 1;
        /// <summary>
        /// 任务步长
        /// </summary>
        public int Step
        {
            get
            {
                return istep;
            }
            set
            {
                istep = value;
            }
        }
        private int icurrent = 0;
        /// <summary>
        /// 当前位置
        /// </summary>
        public int Current
        {
            get
            {
                return icurrent;
            }
            set
            {
                icurrent = value;
            }
        }
        private int ierrorcount = 0;
        /// <summary>
        /// 错误条数
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return ierrorcount;
            }
            set
            {
                ierrorcount = value;
            }
        }
        private int itimeout = 5000;
        /// <summary>
        /// 超时设置 默认为 5000
        /// </summary>
        public int TimeOut
        {
            get
            {
                return itimeout;
            }
            set
            {
                itimeout = value;
            }
        }
        private int ithreadcount = 5;
        /// <summary>
        /// 线程数 默认为 5
        /// </summary>
        public int ThreadCount
        {
            get
            {
                return ithreadcount;
            }
            set
            {
                ithreadcount = value;
            }
        }
        private TaskStatus fstatus = TaskStatus.Pause;
        /// <summary>
        /// 线程状态 默认为Pause 暂停/停止
        /// </summary>
        public TaskStatus Status
        {
            get
            {
                return fstatus;
            }
            set
            {
                fstatus = value;
            }
        }
        private TaskType ftype = TaskType.Customer;
        /// <summary>
        /// 任务类型，默认为 用户自定义
        /// </summary>
        public TaskType Type
        {
            get
            {
                return ftype;
            }
            set
            {
                ftype = value;
            }
        }
        private string flogfile = string.Empty;
        /// <summary>
        /// 日志文件名称
        /// </summary>
        public string LogFile
        {
            get
            {
                return flogfile;
            }
            set
            {
                flogfile = value;
            }
        }
        private string ffilename = string.Empty;
        /// <summary>
        /// 从文件加载任务时，需指定的文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return ffilename;
            }
            set
            {
                ffilename = value;
            }
        }
        private string fcreatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 任务创建时间
        /// </summary>
        public string CreateTime
        {
            get
            {
                return fcreatetime;
            }
        }
    }
}
