using System;

namespace ORM.VSPackage.Identifiers
{
    static class GuidList
    {
        public const string guidORM_VSPackagePkgString = "8fb8f658-e088-4b06-bd4a-87d34fbf8c80";
        public const string guidORM_VSPackageCmdSetString = "b0294d19-2224-436b-b7db-ad80a86f3a22";
        public const string guidToolWindowPersistanceString = "49f96c41-eb09-4c6b-8afd-6b1f2e330f19";
        public const string guidORM_VSPackageEditorFactoryString = "649c9cba-738a-4539-99bf-0bb274a798a2";

        public static readonly Guid guidORM_VSPackageCmdSet = new Guid(guidORM_VSPackageCmdSetString);
        public static readonly Guid guidORM_VSPackageEditorFactory = new Guid(guidORM_VSPackageEditorFactoryString);
    };
}