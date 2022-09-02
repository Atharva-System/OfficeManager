using MockQueryable.Moq;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UnitTests.Mocks
{
    public class MockSkillContext : BaseMockContext
    {
        protected readonly Mock<IApplicationDbContext> mockContext;

        public MockSkillContext()
        {
            mockContext = GetSkillDbContext();
        }

        protected Mock<IApplicationDbContext> GetSkillDbContext()
        {
            var mockContext = new Mock<IApplicationDbContext>();

            //Skill
            var skills = GetSkills();
            mockContext.Setup(r => r.Skill).Returns(skills.AsQueryable().BuildMockDbSet().Object);
            mockContext.Setup(m => m.Skill.AddAsync(It.IsAny<Skill>(), default)).Callback<Skill, CancellationToken>((s, token) =>
            {
                skills.Add(s);
            });

            //SkillRate
            mockContext.Setup(r => r.SkillRate).Returns(GetSkillRates().AsQueryable().BuildMockDbSet().Object);

            //SkillLevel
            mockContext.Setup(r => r.SkillLevel).Returns(GetSkillLevel().AsQueryable().BuildMockDbSet().Object);

            return mockContext;
        }

        protected List<Skill> GetSkills()
        {
            return new List<Skill>{
             new Skill{
                 Id = 1,
                 Name = "Project Management",
                 Description="Project Management",
                 IsActive = true
             },
             new Skill{
                 Id = 2,
                 Name = ".Net Core",
                 Description=".Net Core",
                 IsActive = true
             },
         };
        }

        protected List<SkillRate> GetSkillRates()
        {
            return new List<SkillRate>{
             new SkillRate{
                 Id = 1,
                 Name = "1",
                 Description="1",
                 IsActive = true
             },
             new SkillRate{
                 Id = 2,
                 Name = "2",
                 Description="2",
                 IsActive = true
             },
         };
        }

        protected List<SkillLevel> GetSkillLevel()
        {
            return new List<SkillLevel>{
             new SkillLevel{
                 Id = 1,
                 Name = "Beginner",
                 Description="Beginner",
                 IsActive = true
             },
             new SkillLevel{
                 Id = 2,
                 Name = "Intermediate",
                 Description="Intermediate",
                 IsActive = true
             },
             new SkillLevel{
                 Id = 3,
                 Name = "Expert",
                 Description="Expert",
                 IsActive = true
             }
         };
        }

        protected List<Skill> GetTenSkills()
        {
            var skillsList = new List<Skill>();

            for (int i = 1; i <= 10; i++)
            {
                skillsList.Add(new Skill
                {
                    Id = i,
                    Name = "Test " + i,
                    Description = "Test " + i,
                    IsActive = true
                });
            }

            return skillsList;
        }

    }
}
