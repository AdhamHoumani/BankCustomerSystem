using BankCustomerSystem.Services.Finance.Data.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BankCustomerSystem.Services.Finance.Data.CustomModels
{
    public class RepoDbQueryProperties
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public QueryField[] Where { get; set; } = { };
        public OrderField[] OrderBy { get; set; } = { };

        public RepoDbQueryProperties()
        {
            Take = Take;
            Skip = Skip;
            Where = Where;
            OrderBy = OrderBy;
        }

        public RepoDbQueryProperties(QueryProperties gridProperties)
        {
            Take = gridProperties.PageSize;
            Skip = gridProperties.PageSize * (gridProperties.PageIndex - 1);
            Where = CreateWhereCondition(gridProperties.Where);
            OrderBy = CreateOrderBy(gridProperties.OrderBy);
        }

        public QueryField[] CreateWhereCondition(List<WhereField> whereFields)
        {
            List<QueryField> queryFieldsList = new List<QueryField>();

            if (whereFields != null)
            {
                foreach (var item in whereFields)
                {
                    Operation operation = GetOperationFromEnum(item.Operation);
                    if (operation > 0)
                    {
                        if (operation == Operation.In || operation == Operation.NotIn)
                        {
                            var val = item?.Value?.ToString();
                            string[] array = JsonConvert.DeserializeObject<string[]>(val);
                            queryFieldsList.Add(new QueryField(item.Name, operation, array));
                        }
                        else
                        {
                            var val = item.Value != null ? item.Value.ToString() : item.Value;
                            queryFieldsList.Add(new QueryField(item.Name, operation, val));
                        }
                    }
                }
            }
            return queryFieldsList.ToArray();
        }

        public OrderField[] CreateOrderBy(List<OrderByField> orderByFields)
        {
            List<OrderField> orderFields = new List<OrderField>();

            if (orderByFields != null)
            {
                foreach (var item in orderByFields)
                {
                    Order order = GetOrderByFromEnum(item.Direction);
                    if (order > 0)
                    {
                        orderFields.Add(new OrderField(item.Name, order));
                    }
                }
            }
            return orderFields.ToArray();
        }

        public Operation GetOperationFromEnum(int enumValue)
        {
            switch (enumValue)
            {
                case (int)OperationEnum.Equal:
                    return Operation.Equal;
                case (int)OperationEnum.NotEqual:
                    return Operation.NotEqual;
                case (int)OperationEnum.Greater:
                    return Operation.GreaterThan;
                case (int)OperationEnum.Less:
                    return Operation.LessThan;
                case (int)OperationEnum.GreaterOrEqual:
                    return Operation.GreaterThanOrEqual;
                case (int)OperationEnum.LessOrEqual:
                    return Operation.LessThanOrEqual;
                case (int)OperationEnum.Like:
                    return Operation.Like;
                case (int)OperationEnum.NotLike:
                    return Operation.NotLike;
                case (int)OperationEnum.In:
                    return Operation.In;
                case (int)OperationEnum.NotIn:
                    return Operation.NotIn;
                default:
                    return 0;
            }
        }

        public Order GetOrderByFromEnum(int enumValue)
        {
            switch (enumValue)
            {
                case (int)OrderByEnum.Ascending:
                    return Order.Ascending;
                case (int)OrderByEnum.Descending:
                    return Order.Descending;
                default:
                    return 0;
            }
        }
    }
}