﻿using Application.Repositories;
using Application.Repositories.BankCardRepository;
using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierCommentRepository;
using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Application.Repositories.OrderRatingRepository;
using Application.Repositories.OrderRepository;
using Application.Repositories.RestaurantCommentRepository;
using Application.Repositories.RestaurantRepository;
using Application.Repositories.UserRepository;
using Application.Repositories.WorkerRepository;
using Persistence.Repositories.WorkerRepository;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IReadCourierRepository readCourierRepository, IWriteCourierRepository writeCourierRepository, IReadCategoryRepository readCategoryRepository, IWriteCategoryRepository writeCategoryRepository, IReadFoodRepository readFoodRepository, IWriteFoodRepository writeFoodRepository, IReadRestaurantRepository readRestaurantRepository, IWriteRestaurantRepository writeRestaurantRepository, IReadUserRepository readUserRepository, IWriteUserRepository writeUserRepository, IReadOrderRepository readOrderRepository, IWriteOrderRepository writeOrderRepository, IReadCourierCommentRepository readCourierCommentRepository, IWriteCourierCommentRepository writeCourierCommentRepository, IReadRestaurantCommentRepository readRestaurantCommentRepository, IWriteRestaurantCommentRepository writeRestaurantCommentRepository, IReadOrderRatingRepository readOrderRatingRepository, IWriteOrderRatingRepository writeOrderRatingRepository, IReadWorkerRepository readWorkerRepository, IWriteWorkerRepository writeWorkerRepository, IReadBankCardRepository readBankCardRepository, IWriteBankCardRepository writeBankCardRepository)
    {
        ReadCourierRepository = readCourierRepository;
        WriteCourierRepository = writeCourierRepository;
        ReadCategoryRepository = readCategoryRepository;
        WriteCategoryRepository = writeCategoryRepository;
        ReadFoodRepository = readFoodRepository;
        WriteFoodRepository = writeFoodRepository;
        ReadRestaurantRepository = readRestaurantRepository;
        WriteRestaurantRepository = writeRestaurantRepository;
        ReadUserRepository = readUserRepository;
        WriteUserRepository = writeUserRepository;
        ReadOrderRepository = readOrderRepository;
        WriteOrderRepository = writeOrderRepository;
        ReadCourierCommentRepository = readCourierCommentRepository;
        WriteCourierCommentRepository = writeCourierCommentRepository;
        ReadRestaurantCommentRepository = readRestaurantCommentRepository;
        WriteRestaurantCommentRepository = writeRestaurantCommentRepository;
        ReadOrderRatingRepository = readOrderRatingRepository;
        WriteOrderRatingRepository = writeOrderRatingRepository;
        ReadWorkerRepository = readWorkerRepository;
        WriteWorkerRepository = writeWorkerRepository;
        ReadBankCardRepository = readBankCardRepository;
        WriteBankCardRepository = writeBankCardRepository;
    }

    public IReadCourierRepository ReadCourierRepository { get; }
    public IWriteCourierRepository WriteCourierRepository { get; }
    public IReadCategoryRepository ReadCategoryRepository { get; }
    public IWriteCategoryRepository WriteCategoryRepository { get; }
    public IReadFoodRepository ReadFoodRepository { get; }
    public IWriteFoodRepository WriteFoodRepository { get; }
    public IReadRestaurantRepository ReadRestaurantRepository { get; }
    public IWriteRestaurantRepository WriteRestaurantRepository { get; }
    public IReadUserRepository ReadUserRepository { get; }
    public IWriteUserRepository WriteUserRepository { get; }
    public IReadOrderRepository ReadOrderRepository { get; }
    public IWriteOrderRepository WriteOrderRepository { get; }
    public IReadCourierCommentRepository ReadCourierCommentRepository { get; }
    public IWriteCourierCommentRepository WriteCourierCommentRepository { get; }
    public IReadRestaurantCommentRepository ReadRestaurantCommentRepository { get; }
    public IWriteRestaurantCommentRepository WriteRestaurantCommentRepository { get; }
    public IReadOrderRatingRepository ReadOrderRatingRepository { get; }
    public IWriteOrderRatingRepository WriteOrderRatingRepository { get; }
    public IReadWorkerRepository ReadWorkerRepository { get; }
    public IWriteWorkerRepository WriteWorkerRepository { get; }
    public IReadBankCardRepository ReadBankCardRepository { get; }
    public IWriteBankCardRepository WriteBankCardRepository { get; }
}