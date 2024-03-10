﻿using Application.Repositories.RestaurantCommentRepository;
using Persistence.Context;
using Persistence.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.RestaurantCommentRepository
{
    internal class WriteRestaurantCommentRepository: WriteRepository<RestaurantComment> , IWriteRestaurantCommentRepository
    {
        public WriteRestaurantCommentRepository(AppDbContext context) : base(context)
        {

        }
    }
}