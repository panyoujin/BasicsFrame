using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BF.Common.Extensions
{
    public static class ObjectContextExtension
    {
        public static T AttachExistedEntity<T>(this ObjectContext context, T entity)
            where T : EntityObject
        {
            EdmEntityTypeAttribute entityTypeAttr = (EdmEntityTypeAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EdmEntityTypeAttribute), false);
            if (entityTypeAttr == null)
                throw new NotSupportedException("T is not an entity.");
            string entityFullname = context.DefaultContainerName + "." + entityTypeAttr.Name;
            entity.EntityKey = new System.Data.EntityKey(entityFullname,
            from p in typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
            where p.GetGetMethod(false) != null
            let attribute = (EdmScalarPropertyAttribute)Attribute.GetCustomAttribute(p, typeof(EdmScalarPropertyAttribute))
            where attribute != null && attribute.EntityKeyProperty
            select new KeyValuePair<string, object>(p.Name, p.GetValue(entity, null)));
            context.Attach(entity);
            //context.ApplyPropertyChanges(entityTypeAttr.Name, entity);
            context.ApplyCurrentValues(entityTypeAttr.Name, entity);
            return entity;
        }
        

        public static EntityCollection<TElement> CreateCollection<T, TElement>(this T entity, Expression<Func<T, EntityCollection<TElement>>> expr, params TElement[] items)
            where T : EntityObject
            where TElement : EntityObject
        {
            if (expr.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Expression is not correct.", "expr");
            var member = ((MemberExpression)expr.Body).Member;
            PropertyInfo pi = member as PropertyInfo;
            if (pi == null)
                throw new ArgumentException("Expression is not correct.", "expr");
            EdmRelationshipNavigationPropertyAttribute attribute = (EdmRelationshipNavigationPropertyAttribute)Attribute.GetCustomAttribute(pi, typeof(EdmRelationshipNavigationPropertyAttribute));
            EntityCollection<TElement> result = new EntityCollection<TElement>();
            RelationshipManager rm = RelationshipManager.Create(entity);
            rm.InitializeRelatedCollection(attribute.RelationshipName, attribute.TargetRoleName, result);
            foreach (var item in items)
                result.Add(item);
            return result;
        }
    }
}
