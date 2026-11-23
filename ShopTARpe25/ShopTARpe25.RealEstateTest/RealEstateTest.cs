using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange 
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            //Act Siin tuleb kutsuda RealEstate create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);
        }
        //Teha test, kus otsib ankeeti id alusel, aga aeda ei ole andmebaasis
        public class RealEstateTest2 : TestBase
        {
            [Fact]
            public async Task Should_GetByIdRealEstate_WhenWhenReturnsEqual()
            {
                //Arrange
                Guid databaseGuid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");
                Guid wrongGuid = Guid.NewGuid();

                //Act
                await Svc<IRealEstateServices>().DetailAsync(databaseGuid);

                //Assert
                //Vastupidi loogikat tuleb kasutada
                Assert.NotEqual(wrongGuid, databaseGuid);
            }
        }
        
    }
}
