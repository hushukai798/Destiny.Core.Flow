﻿using AutoMapper;
using Destiny.Core.Flow.Entity;
using Destiny.Core.Flow.Model.Entities.Dictionary;
using Destiny.Core.Flow.Model.Entities.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Destiny.Core.Flow.Dtos.DataDictionnary
{
    /// <summary>
    /// 加载一个数据字典dto
    /// </summary>
    [AutoMap(typeof(DataDictionaryEntity))]
    public class DataDictionnaryLoadDto : OutputDto<Guid>
    {
        /// <summary>
        /// 数据字典标题
        /// </summary>
        [DisplayName("数据字典标题")]
        public string Title { get; set; }

        /// <summary>
        /// 数据字典值
        /// </summary>
        [DisplayName("数据字典值")]
        public string Value { get; set; }

        /// <summary>
        /// 数据字典备注
        /// </summary>
        [DisplayName("数据字典备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 数据字典父级
        /// </summary>
        [DisplayName("数据字典父级")]
        public Guid ParentId { get; set; } = Guid.Empty;

        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int Sort { get; set; }

        /// <summary>
        ///获取或设置 编码
        /// </summary>
        [DisplayName("编码")]
        public string Code { get; set; }
        /// <summary>
        /// id
        /// </summary>
        [DisplayName("Id")]
        public Guid Id { get; set; }
    }
}