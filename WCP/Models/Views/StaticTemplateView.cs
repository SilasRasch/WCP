﻿
namespace WCPShared.Models.Views
{
    public class StaticTemplateView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TemplateImgOne { get; set; } = string.Empty;
        public string TemplateImgTwo { get; set; } = string.Empty;
        public string ExampleImg { get; set; } = string.Empty;

        public StaticTemplateView(StaticTemplate obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            TemplateImgOne = obj.TemplateImgOne;
            TemplateImgTwo = obj.TemplateImgTwo;
            ExampleImg = obj.ExampleImg;
        }
    }
}
