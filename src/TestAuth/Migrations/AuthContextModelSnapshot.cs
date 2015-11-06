using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using TestAuth.AuthModels;

namespace TestAuth.Migrations
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TestAuth.AuthModels.Role", b =>
                {
                    b.Property<int>("RoleID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("RoleID");
                });

            modelBuilder.Entity("TestAuth.AuthModels.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomProperty");

                    b.Property<string>("Email");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("RoleID");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("UserID");
                });

            modelBuilder.Entity("TestAuth.AuthModels.User", b =>
                {
                    b.HasOne("TestAuth.AuthModels.Role")
                        .WithMany()
                        .ForeignKey("RoleID");
                });
        }
    }
}
