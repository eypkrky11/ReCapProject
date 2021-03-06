﻿using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        public IResult Add(Rental rental)
        {
            if (rental.ReturnDate == null)
            {
                return new ErrorResult(Messages.CannotBeRented);
            }
            if (_rentalDal.GetRentalDetailDTOs(c => c.CarId == rental.CarId).Count > 0)
            {
                return new ErrorResult(Messages.NoCar);
            }
            else
            {
                _rentalDal.Add(rental);
                return new SuccessResult();
            }
        }

        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult();
        }
        public IDataResult<Rental> GetById(int id)
        {
            return new SuccessDataResult<Rental>(_rentalDal.Get(p => p.RentalId == id));
        }

        public IResult Insert(Rental entity)
        {
            var getByCarId = _rentalDal.GetAll(p => p.CarId == entity.CarId);

            foreach (var car in getByCarId)
            {
                if (car.ReturnDate == null)
                {
                    return new ErrorResult(Messages.CarUndelivered);
                }
            }

            _rentalDal.Add(entity);
            return new SuccessResult(Messages.RentalAdded);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll());
        }

        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult();
        }

        public IDataResult<List<RentalDetailDTO>> GetRentalDetailDTOs()
        {
            return new SuccessDataResult<List<RentalDetailDTO>>(_rentalDal.GetRentalDetailDTOs());
        }
    }
}
