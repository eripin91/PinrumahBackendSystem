using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PinBackendSystem.Data;

namespace PinBackendSystem.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PinContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PinContext>>()))
            {
                // Look for any movies.
                if (context.Listings.Any())
                {
                    return;   // DB has been seeded
                }

                context.Listings.AddRange(
                    new Listing
                    {
                        Title = "RUMAH DI GADING SERPONG LT 6x12 m2 [HUB: 081377338080] LUQMAN PR 36100",
                        PropertyId = "PR36100",
                        Description = "<p>Kode Listing pinrumah.com :&nbsp;<strong>36100</strong><br /><br />(HUB : LUQMAN 087882787286)<br />(HUB : LUQMAN 081280069222)<br />(HUB : LUQMAN 081377338080)<br /><br />LT 6 x 12 = 72 m2<br />LB 70 m2<br />Bangunan 2 Lantai<br />3 KT + 2 KM ( 1KM di lantai bawah)<br />Air PAM<br />Kondisi rapi sudah renov dan siap huni<br /><br />1,15 M<br /><br />(HUB : LUQMAN 087882787286)<br />(HUB : LUQMAN 081280069222)<br />(HUB : LUQMAN 081377338080)<br /><br />Segera hubungi kami untuk berkonsultasi dan mendapatkan informasi seputar property pada area ini (HOT Listing, harga pasaran dan Lokasi terbaik)<br /><br />LQM</p>",
                        Address = "Gading Serpong",                          
                        ImagePath1 = "https://i2.au.reastatic.net/1000x750/e0b9377f686dc265fc9c423a71077be7083253033d5e0d62e4d3cce202757135/image.jpg",
                        ImagePath2= "https://i2.au.reastatic.net/360x270/dfd2d852b28cd9b49ff74d156f6cf14f38a5b2e82f4c68e131873bce8340c527/image.jpg",
                        ImagePath3= "https://i2.au.reastatic.net/360x270/a0420bfb8b805b9245c5d001b6cdbf2021783fce68221943cfba013b7a38d4bf/image.jpg",
                        ImagePath4 = "https://i2.au.reastatic.net/360x270/75b8e332a4a76725a4583f91cafe904e48156ea4f98d2539ba60c53b4c6e43eb/image.jpg",
                        ImagePath5 = "https://i2.au.reastatic.net/360x270/231c018499a65659dab17ef062438a9621ed8e3cd7f8cd870594fd4717adef77/image.jpg",
                        LandSize = 72,
                        BuildingSize = 70,
                        PropertyType = "Rumah",
                        NoOfBed=3,
                        NoOfBath=2,
                        NoOfFloor=2,
                        NoOfGarage=0,
                        PropertyCondition="Baru",
                        Price=1150000000,
                        //Features="AC,Balkon,Granit",
                        AgentName="Edward",
                        AgentEmail="Edward@pinrumah.com",
                        AgentMobileNo="628170929898",
                        OwnerName="Joko",
                        OwnerMobileNo="081708929812",
                        CreatedOn=DateTime.Now,
                        CreatedBy="011231asfsdfasdf",
                    },
                    new Listing
                    {
                        Title = "DIJUAL APARTEMEN DI RAWA BUAYA LT 26 M2 LB 26 M2, JAKARTA BARAT (HUB EDWARD 081280069222) PR",
                        PropertyId = "PR36101",
                        Description = "<p>Kode Listing pinrumah.com :&nbsp;<strong>36101</strong><br /><br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br /><br />GRATIS Konsultasi KPR (BCA, PANIN, CIMB, BNI, Danamon, Permata, dll)<br /><br />SPEK :<br /><br />- LUAS TANAH :26 M2<br />- Luas Bangunan : 26 M2<br />KT 1<br />KM 1<br /><br />HARGA : 540 JT<br /><br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br /><br />Segera hubungi kami untuk membantu dan mendapatkan informasi mengenai properti di area ini (Listing HOT, harga pasaran dan lokasi terbaik)<br /><br />SHR,SHR,RW</p>",
                        Address = "Rawa Buaya",
                        ImagePath1= "https://eriblobstorage.blob.core.windows.net/uploads/2020-05-20/gbr%20rumah2.jpg",
                        ImagePath2 = "https://eriblobstorage.blob.core.windows.net/uploads/2020-05-20/gbr%20rumah1.jpg",
                        LandSize = 26,
                        BuildingSize = 26,
                        PropertyType = "Apartemen",
                        NoOfBed = 1,
                        NoOfBath = 1,
                        NoOfFloor = 1,
                        NoOfGarage = 0,
                        PropertyCondition = "Baru",
                        Price = 540000000,
                        //Features = "Gordyn,PAM,Swimming pool",
                        AgentName = "Edward",
                        AgentEmail = "Edward@pinrumah.com",
                        AgentMobileNo = "628170929898",
                        OwnerName = "Roni",
                        OwnerMobileNo = "081708929812",
                        CreatedOn = DateTime.Now,
                        CreatedBy = "011231asfsdfasdf",
                    },
                    new Listing
                    {
                        Title = "DIJUAL APARTEMEN DI DADAP LT 33 M2 LB 29 M2, TANGERANG (HUB EDWARD 081280069222) PR",
                        PropertyId = "PR36102",
                        Description = "<p><p>Kode Listing pinrumah.com :&nbsp;<strong>36102</strong><br /><br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br /><br />GRATIS Konsultasi KPR (BCA, PANIN, CIMB, BNI, Danamon, Permata, dll)<br /><br />SPEK :<br /><br />- LUAS TANAH :33 M2<br />- Luas Bangunan : 29 M2<br />KT 1<br />KM 1<br /><br />HARGA : 356 JT<br /><br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br /><br />Segera hubungi kami untuk membantu dan mendapatkan informasi mengenai properti di area ini (Listing HOT, harga pasaran dan lokasi terbaik)<br /><br />SHR,SHR,RW</p>",
                        Address = "Tangerang",
                        ImagePath1= "https://eriblobstorage.blob.core.windows.net/uploads/2020-05-20/gbr%20rumah3.jpg",
                        LandSize = 33,
                        BuildingSize = 29,
                        PropertyType = "Apartemen",
                        NoOfBed = 1,
                        NoOfBath = 1,
                        NoOfFloor = 1,
                        NoOfGarage = 1,
                        PropertyCondition = "Baru",
                        Price = 356000000,
                        //Features = "AC,Gordyn,Garasi",
                        AgentName = "Edward",
                        AgentEmail = "Edward@pinrumah.com",
                        AgentMobileNo = "628170929898",
                        OwnerName = "Susi",
                        OwnerMobileNo = "081708929812",
                        CreatedOn = DateTime.Now,
                        CreatedBy = "011231asfsdfasdf",
                    },
                    new Listing
                    {
                        Title = "DIJUAL APARTEMEN DI KAPUK LT 22 M2 LB 22 M2, JAKARTA BARAT (HUB EDWARD 081280069222) PR",
                        PropertyId = "PR36103",
                        Description = "<p>Kode Listing pinrumah.com :&nbsp;<strong>36103</strong><br /><br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br /><br />GRATIS Konsultasi KPR (BCA, PANIN, CIMB, BNI, Danamon, Permata, dll)<br /><br />SPEK :<br /><br />- LUAS TANAH :22 M2<br />- Luas Bangunan : 22 M2<br />KT 1<br />KM 1<br /><br />HARGA : 260 JT<br /><br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br />(HUB: EDWARD 081280069222)<br /><br />Segera hubungi kami untuk membantu dan mendapatkan informasi mengenai properti di area ini (Listing HOT, harga pasaran dan lokasi terbaik)<br /><br />SHR,SHR,RW</p>",
                        Address = "Kapuk",
                        ImagePath1= "https://eriblobstorage.blob.core.windows.net/uploads/2020-05-20/gbr%20rumah4.jpg",
                        LandSize = 22,
                        BuildingSize = 22,
                        PropertyType = "Apartemen",
                        NoOfBed = 1,
                        NoOfBath = 1,
                        NoOfFloor = 1,
                        NoOfGarage = 0,
                        PropertyCondition = "Baru",
                        Price = 260000000,
                        //Features = "AC,Gordyn,Garasi",
                        AgentName = "Edward",
                        AgentEmail = "Edward@pinrumah.com",
                        AgentMobileNo = "628170929898",
                        OwnerName = "Ratna",
                        OwnerMobileNo = "081708929812",
                        CreatedOn = DateTime.Now,
                        CreatedBy = "011231asfsdfasdf",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
