﻿using System.Globalization;
using _0_Framework.Application;
using ShopManagement.Application.Contract.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application {
    public class SlideApplication: ISlideApplication {
        private readonly ISlideRepository _slideRepository;

        public SlideApplication (ISlideRepository slideRepository) {
            _slideRepository = slideRepository;
        }

        public OperationResult Create (CreateSlide command) {
            var operation = new OperationResult();
            var slide = new Slide(command.Picture, command.PictureAlt, command.PictureTitle, command.Heading,
                command.Title, command.Text, command.Link, command.BtnText);
            _slideRepository.Create(slide);
            _slideRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit (EditSlide command) {
            var operation = new OperationResult();
            var slide = _slideRepository.GetById(command.Id);
            if(slide == null) {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            slide.Edit(command.Picture, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.Link, command.BtnText);
            _slideRepository.SaveChanges();
            return operation.Succeeded();
        }

        public EditSlide? GetDetails (long id) {
            return _slideRepository.GetDetails(id);
        }

        public OperationResult Remove (long id) {
            var operation = new OperationResult();
            var slide = _slideRepository.GetById(id);
            if(slide == null) {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }
            slide.Remove();
            _slideRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Restore (long id) {
            var operation = new OperationResult();
            var slide = _slideRepository.GetById(id);
            if(slide == null) {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }
            slide.Restore();
            _slideRepository.SaveChanges();
            return operation.Succeeded();
        }

        public List<SlideViewModel> GetAll () {
            return _slideRepository.GetAll().Select(x => new SlideViewModel {
                Picture = x.Picture,
                Heading = x.Heading,
                Id = x.Id,
                Title = x.Title,
                IsRemoved = x.IsRemoved,
                CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture),
                
            }).OrderByDescending(x => x.Id).ToList();
        }
    }
}