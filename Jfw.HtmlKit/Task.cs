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
        /// Task����
        /// </summary>
        public Task()
        { }
        private string fname = string.Empty;
        /// <summary>
        /// ��������
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
        /// ��ʼλ��
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
        /// ����λ��
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
        /// ���񲽳�
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
        /// ��ǰλ��
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
        /// ��������
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
        /// ��ʱ���� Ĭ��Ϊ 5000
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
        /// �߳��� Ĭ��Ϊ 5
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
        /// �߳�״̬ Ĭ��ΪPause ��ͣ/ֹͣ
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
        /// �������ͣ�Ĭ��Ϊ �û��Զ���
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
        /// ��־�ļ�����
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
        /// ���ļ���������ʱ����ָ�����ļ�����
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
        /// ���񴴽�ʱ��
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
