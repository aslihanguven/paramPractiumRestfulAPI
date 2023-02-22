using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using paramRestfulAPI;
using paramRestfulAPI.Data;
using paramRestfulAPI.Models;
using paramRestfulAPI.Models.DTO;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(option =>
     option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* Minimal API
app.MapGet("/helloworld/{id:int}", (int id) =>
{
    return Results.BadRequest("Exception");
    return Results.Ok("Id" + id);
});
app.MapPost("/helloworld2", () => "Hello World 2"); */

// Get All Endpoint
// with logger DI
app.MapGet("/api/coupon", (ILogger<Program> _logger) => {
    APIResponse response = new();
    _logger.Log(LogLevel.Information, "Getting Coupons");
    response.Result = Store.couponList;
    response.IsUsable = true;
    response.StatusCode =HttpStatusCode.OK;
    return Results.Ok(response);

    }).WithName("GetCoupons").Produces<APIResponse>(200);

app.MapGet("/api/coupon/{id:int}", (ILogger < Program > _logger, int id) => {
    APIResponse response = new();
    response.Result = Store.couponList;
    response.IsUsable = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

}).WithName("GetCoupon").Produces<APIResponse>(200);

app.MapPost("/api/coupon",async (IMapper _mapper, IValidator<CouponCreateDTO> _validation ,[FromBody] CouponCreateDTO coupon_C_DTO) => {

    APIResponse response = new() {IsUsable = false, StatusCode = HttpStatusCode.BadRequest };

    var validationResult =await _validation.ValidateAsync(coupon_C_DTO);
    if(!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }
    if(Store.couponList.FirstOrDefault(u=>u.Name.ToLower() == coupon_C_DTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Coupon Name Already Exists");
        return Results.BadRequest(response);
        
    }

    Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);

    coupon.Id = Store.couponList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
    Store.couponList.Add(coupon);
    CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);
    //return Results.CreatedAtRoute("GetCoupon",new {id= coupon.Id}, couponDTO);

    response.Result = couponDTO;
    response.IsUsable = true;
    response.StatusCode = HttpStatusCode.Created;
    return Results.Ok(response);
    //return Results.Ok(coupon);
    //return Results.Created($"/api/coupon/{coupon.Id}", coupon);

}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

app.MapPut("/api/coupon", async (IMapper _mapper, IValidator<CouponUpdateDTO> _validation, [FromBody] CouponUpdateDTO coupon_U_DTO) =>
{
    APIResponse response = new() { IsUsable = false, StatusCode = HttpStatusCode.BadRequest };

    var validationResult = await _validation.ValidateAsync(coupon_U_DTO);
    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    Coupon couponFromStore = Store.couponList.FirstOrDefault(u=> u.Id == coupon_U_DTO.Id);
    couponFromStore.IsUsable = coupon_U_DTO.IsUsable;
    couponFromStore.Name = coupon_U_DTO.Name;
    couponFromStore.Percent = coupon_U_DTO.Percent;
    couponFromStore.Updated = DateTime.Now;


    response.Result = _mapper.Map<CouponDTO>(couponFromStore);
    response.IsUsable = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

}).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400); ;

app.MapDelete("/api/coupon/{id:int}", (int id) => {
    
    APIResponse response = new() { IsUsable = false, StatusCode = HttpStatusCode.BadRequest };

    Coupon couponFromStore = Store.couponList.FirstOrDefault(u => u.Id == id);
    if(couponFromStore != null)
    {
        Store.couponList.Remove(couponFromStore);
        response.IsUsable = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid ID");
        return Results.BadRequest(response);
    }
   
});


app.UseHttpsRedirection();
app.Run();

