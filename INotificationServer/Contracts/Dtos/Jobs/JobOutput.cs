﻿using Quartz;

namespace INotificationServer.Contracts.Dtos.Jobs
{
    public class JobOutput
    {
            /// <summary>
            /// 任务名称
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            /// 下次执行时间
            /// </summary>
            public DateTime? NextFireTime { get; set; }

            /// <summary>
            /// 上次执行时间
            /// </summary>
            public DateTime? PreviousFireTime { get; set; }

            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime BeginTime { get; set; }

            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime? EndTime { get; set; }

            /// <summary>
            /// 上次执行的异常信息
            /// </summary>
            public string? LastErrMsg { get; set; }

            /// <summary>
            /// 任务状态
            /// </summary>
            public TriggerState TriggerState { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string? Description { get; set; }

            /// <summary>
            /// 显示状态
            /// </summary>
            public string DisplayState
            {
                get
                {
                    var state = string.Empty;
                    switch (TriggerState)
                    {
                        case TriggerState.Normal:
                            state = "正常";
                            break;
                        case TriggerState.Paused:
                            state = "暂停";
                            break;
                        case TriggerState.Complete:
                            state = "完成";
                            break;
                        case TriggerState.Error:
                            state = "异常";
                            break;
                        case TriggerState.Blocked:
                            state = "阻塞";
                            break;
                        case TriggerState.None:
                            state = "不存在";
                            break;
                        default:
                            state = "未知";
                            break;
                    }
                    return state;
                }
            }

            /// <summary>
            /// 时间间隔
            /// </summary>
            public string? Interval { get; set; }

            /// <summary>
            /// 触发地址
            /// </summary>
            public string? TriggerAddress { get; set; }
            public string? RequestType { get; set; }
            /// <summary>
            /// 已经执行的次数
            /// </summary>
            public long RunNumber { get; set; }
            public string? JobType { get; set; }
    }
}