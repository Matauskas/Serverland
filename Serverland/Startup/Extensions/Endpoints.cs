using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Serverland.Data;
using Serverland.Data.DatabaseObjects;
using Serverland.Data.Entities;
using Serverland.Examples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Xml;
namespace Serverland.Extensions;


public static class Endpoints
{
    public static void AddCategoryApi(this WebApplication app)
    {
        var categoryGroups = app.MapGroup("/api").AddFluentValidationAutoValidation().WithTags("category");
        
        categoryGroups.MapGet("/category", async (ServerDbContext dbContext) =>
        {
            return (await dbContext.Categories.ToListAsync()).Select(category => category.ToDto());
        })
        .WithName("getAllCategories")
        .WithMetadata(new SwaggerOperationAttribute("Get a list of categories", "A list of categories"))
        .Produces<List<CategoryDto>>(StatusCodes.Status200OK);

        categoryGroups.MapGet("/category/{categoryId}", async (int categoryId, ServerDbContext dbContext) =>
        {
            var category = await dbContext.Categories.FindAsync(categoryId);
            return category == null ? Results.NotFound() : TypedResults.Ok(category.ToDto());
        })
        .WithName("getCategory")
        .WithMetadata(new SwaggerOperationAttribute("Get a caterogy by id", "Returns a category based on the provided ID."))
        .Produces<CategoryDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        categoryGroups.MapPost("/category", async (CreateCategoryDto dto, ServerDbContext dbContext) =>
        {
            var category = new Category{Manifacturer = dto.manifacturer, ServerType = dto.serverType};
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"api/category/{category.Id}", category.ToDto());
        })
        .WithName("createCategory")
        .WithMetadata(new SwaggerOperationAttribute("Create category", "Creates a new category with the given data and returns the created post."))
        .Produces<CategoryDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        categoryGroups.MapPut("/category/{categoryId}", async (int categoryId, UpdatedCategoryDto dto, ServerDbContext dbContext) =>
            {
                var category = await dbContext.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return Results.NotFound();
                }

                category.Manifacturer = dto.manifacturer;

                dbContext.Categories.Update(category);
                await dbContext.SaveChangesAsync();

                return TypedResults.Ok(category.ToDto());

            })
            .WithName("updateCategory")
            .WithMetadata(new SwaggerOperationAttribute("Update category by id",
                "Updates the manifacturer of an existing category with the given ID."))
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


        categoryGroups.MapDelete("/category/{categoryId}", async (int categoryId, ServerDbContext dbContext) =>
        {
            var category = await dbContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return Results.NotFound();
            }
    
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.NoContent();
        })
        .WithName("deleteCategory")
        .WithMetadata(new SwaggerOperationAttribute("Delete a category", "Deletes the category with the given ID."))
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
    
    
    public static void AddServerApi(this WebApplication app)
    {
        var serverGroups = app.MapGroup("/api/category/{categoryId}").AddFluentValidationAutoValidation().WithTags("server");
        
        serverGroups.MapGet("/server", async (int categoryId, ServerDbContext dbContext) =>
        { 
            var forum = await dbContext.Servers.FindAsync(categoryId);
           if (forum == null)
           {
                return Results.NotFound();
           }
           return Results.Ok((await dbContext.Servers.ToListAsync())
               .Where(post => categoryId == post.categoryId)
               .Select(post => post.ToDto()));
        })
        .WithName("GetAllPosts")
        .WithMetadata(new SwaggerOperationAttribute("Get All Posts", "Returns a list of all posts."))
        .Produces<List<ServerDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        serverGroups.MapGet("/server/{serverId}", async (int categoryId, int serverId, ServerDbContext dbContext) =>
        {
            var post = await dbContext.Servers.FindAsync(serverId);
            return post == null || post.categoryId != categoryId ? Results.NotFound() : TypedResults.Ok(post.ToDto());
        })
        .WithName("GetPostById")
        .WithMetadata(new SwaggerOperationAttribute("Get Post by ID", "Returns a post based on the provided ID."))
        .Produces<ServerDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        serverGroups.MapPost("/server", async (int categoryId, CreateServerDto dto, ServerDbContext dbContext) => 
            { 
                var forum = await dbContext.Categories.FindAsync(categoryId);
                if (forum == null)
                {
                    return Results.NotFound();
                }
            var server = new Server{Model = dto.model, categoryId = categoryId, Disk_Count = dto.disk_count, Generation = dto.generation, Weight = dto.weight, OS = dto.os};
            dbContext.Servers.Add(server);

            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"api/category/{categoryId}/server/{server.Id}", server.ToDto());
        })
        .WithName("CreateServer")
        .WithMetadata(new SwaggerOperationAttribute("Create a new post", "Creates a new post with the given data and returns the created post."))
        .Produces<ServerDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        serverGroups.MapPut("/server/{serverId}", async (int categoryId, UpdatedServerDto dto, int serverId, ServerDbContext dbContext) =>
        {
            var post = await dbContext.Servers.FindAsync(serverId);
            if (post == null || post.categoryId != categoryId )
            {
                return Results.NotFound();
            }

            post.Model = dto.model;

            dbContext.Servers.Update(post);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.Ok(post.ToDto());

        })
        .WithName("UpdatePost")
        .WithMetadata(new SwaggerOperationAttribute("Update an existing post", "Updates the description of an existing post with the given ID."))
        .Produces<ServerDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        serverGroups.MapDelete("/server/{serverId}", async (int categoryId, int serverId, ServerDbContext dbContext) =>
        {
            var post = await dbContext.Servers.FindAsync(serverId);
            if (post == null || post.categoryId != categoryId)
            {
                return Results.NotFound();
            }
    
            dbContext.Servers.Remove(post);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.NoContent();
        })
        .WithName("DeletePost")
        .WithMetadata(new SwaggerOperationAttribute("Delete a post", "Deletes the post with the given ID."))
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }

    public static void AddPartApi(this WebApplication app)
    {
        var commentsGroups = app.MapGroup("/api/category/{categoryId}/server/{serverId}").AddFluentValidationAutoValidation()
            .WithTags("part");

        commentsGroups.MapGet("/part", async (int serverId, int categoryId, ServerDbContext dbContext) =>
        {
            var forum = await dbContext.Categories.FindAsync(categoryId);
            var post = await dbContext.Servers.FindAsync(serverId);
            if (forum == null|| post == null)
            {
                return Results.NotFound();
            }
            return Results.Ok((await dbContext.Parts.ToListAsync())
                .Where(comment =>  serverId == comment.serverId)
                .Select(comment => comment.ToDto()));
        })
        .WithName("GetAllparts")
        .WithMetadata(new SwaggerOperationAttribute("Get All parts", "Returns a list of all comments."))
        .Produces<List<PartDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        commentsGroups.MapGet("/part/{partId}", async (int partId, int categoryId, int serverId, ServerDbContext dbContext) =>
        {
            var comment = await dbContext.Parts.FindAsync(partId);
            return comment == null || comment.serverId != serverId ? Results.NotFound() : TypedResults.Ok(comment.ToDto());
        })
        .WithName("GetCommentById")
        .WithMetadata(new SwaggerOperationAttribute("Get comment by ID", "Returns a comment based on the provided ID."))
        .Produces<PartDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        commentsGroups.MapPost("/part/", async (int serverId, int categoryId, CreatePartDto dto, ServerDbContext dbContext) => 
            { 
                var post = await dbContext.Servers.FindAsync(serverId);
                if (post == null)
                {
                    return Results.NotFound();
                }
            var comment = new Part{CPU = dto.cpu, RAM = dto.ram, Raid = dto.raid, Network = dto.network, SSD = dto.ssd, HDD = dto.hdd, PSU = dto.psu, Rails = dto.rails, serverId = dto.serverId};
            dbContext.Parts.Add(comment);

            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"api/category/{categoryId}/server/{serverId}/part/{comment.Id}", comment.ToDto());
        })
        .WithName("CreateComment")
        .WithMetadata(new SwaggerOperationAttribute("Create a new comment", "Creates a new comment with the given data and returns the created comment."))
        .Produces<PartDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status422UnprocessableEntity);

        commentsGroups.MapPut("/part/{partId}", async (int partId, int categoryId, UpdatedPartDto dto, int serverId, ServerDbContext dbContext) =>
        {
            var comment = await dbContext.Parts.FindAsync(partId);
            if (comment == null || comment.serverId != serverId )
            {
                return Results.NotFound();
            }

            comment.CPU = dto.CPU;

            dbContext.Parts.Update(comment);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.Ok(comment.ToDto());

        })
        .Accepts<UpdatedPartDto>("application/json")
        .WithName("UpdateComment")
        .WithMetadata(new SwaggerOperationAttribute("Update an existing comment", "Updates the description of an existing comment with the given ID."))
        .Produces<PartDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status422UnprocessableEntity)
        ;

        commentsGroups.MapDelete("/part/{partId}", async (int partId, int categoryId, int serverId, ServerDbContext dbContext) =>
        {
            var comment = await dbContext.Parts.FindAsync(partId);
            if (comment == null || comment.serverId != serverId)
            {
                return Results.NotFound();
            }
    
            dbContext.Parts.Remove(comment);
            await dbContext.SaveChangesAsync();
    
            return TypedResults.NoContent();
        })
        .WithName("DeletePart")
        .WithMetadata(new SwaggerOperationAttribute("Delete a comment", "Deletes the comment with the given ID."))
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}

