using System;
using ChemLab.Data.Entity;
using ChemLab.Data.Repository.Interfaces;
using ChemLab.Models;
using ChemLab.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ChemLab.Services
{
	public class LabPracticeService : ILabPracticeService
	{
		private ILabPracticeRepository _labPracticeRepository;

        private IAccountRepository _accountRepository;

		public LabPracticeService(ILabPracticeRepository labPracticeService, IAccountRepository accountRepository)
		{
			_labPracticeRepository = labPracticeService;
            _accountRepository = accountRepository;
		}

        public async Task<List<LabPracticeViewModel>> GetAllLabPractices(string userid)
        {
            var posts = await _labPracticeRepository.GetAllLabPractices(userid);

            return posts
                .Select(x =>
                    new LabPracticeViewModel()
                    {
                        Id = x.Id,
                        DateOfCreate = x.Date,
                        Name = x.Name,
                        Description = x.Description
                    }).ToList();
        }

        public async Task CreateLabPractice(CreateLabPracticeViewModel createLabPracticeViewModel, string userid)
        {
            var labPractice = new LabPractice(createLabPracticeViewModel);

            var user = await _accountRepository.GetById(userid);

            if (user == null)
            {
                user = new ApplicationUser { Id = userid };
                await _accountRepository.Add(user);
            }

            labPractice.User = user;
            labPractice.Date = DateTime.Now;

            await _labPracticeRepository.Add(labPractice);
        }

    }
}

