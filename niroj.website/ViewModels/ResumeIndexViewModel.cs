using Personal.Domain.Dto;
using System.Collections.Generic;

namespace niroj.website.ViewModels
{
    public class ResumeIndexViewModel
    {
        public List<SettingDto> Settings { get; set; } = new List<SettingDto>();
        public List<WorkExperienceDto> Experiences { get; set; } = new List<WorkExperienceDto>();
        public List<ProjectDto> Projects { get; set; }=new List<ProjectDto>();
        public List<ResumeSkillCategoryViewModel> SkillCategories { get; set; }=new List<ResumeSkillCategoryViewModel>();
    }

    public class ResumeSkillCategoryViewModel
    {
        public string Name { get; set; }

        public List<string> Skills { get; set; } = new List<string>();
    }
}
