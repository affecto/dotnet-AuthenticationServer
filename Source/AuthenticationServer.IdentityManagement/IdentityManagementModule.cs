using System;
using System.Collections.Generic;
using System.Linq;
using Affecto.AuditTrail.Interfaces;
using Affecto.AuditTrail.Interfaces.Model;
using Affecto.AuthenticationServer.IdentityManagement.Configuration;
using Affecto.AuthenticationServer.Infrastructure.Configuration;
using Autofac;
using IdentityServer3.Core.Services;

namespace Affecto.AuthenticationServer.IdentityManagement
{
    public class IdentityManagementModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterInstance(IdentityManagementConfiguration.Settings).As<IIdentityManagementConfiguration>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterModule<Affecto.IdentityManagement.Autofac.ModuleRegistration>();
            builder.RegisterModule<Affecto.IdentityManagement.Store.PostgreSql.ModuleRegistration>();

            builder.RegisterType<AuditTrailMock>().As<IAuditTrailService>();
        }
    }

    internal class AuditTrailMock : IAuditTrailService
    {
        public IEnumerable<IAuditTrailEntry> GetEntries()
        {
            return Enumerable.Empty<IAuditTrailEntry>();
        }

        public IAuditTrailResult GetEntries(IAuditTrailFilter filter)
        {
            return null;
        }

        public IEnumerable<IAuditTrailEntry> GetEntriesForSubject(Guid subjectId)
        {
            return Enumerable.Empty<IAuditTrailEntry>();
        }

        public IAuditTrailEntry GetEntry(Guid auditTrailEntryId)
        {
            return null;
        }

        public IAuditTrailEntry CreateEntry(Guid subjectId, string summary, string subjectName, string userName)
        {
            return null;
        }

        public IAuditTrailEntry CreateEntry(Guid subjectId, Guid userId, string summary, string subjectName, string userName)
        {
            return null;
        }
    }
}