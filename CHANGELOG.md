1. Pridėjome `items` endpoint'us. Kaip suprantame, `items` yra verslo meniu, ką jie gali pasiūlyti, todėl jiems yra aktualu juos redaguoti, matyti jų kainas ir panašiai.
2. Duomenų modelyje `EstablishmentEnum` keičiame į aktyvią duomenų bazės lentą. Kuriai sukursime ir endpoint'us. Juos redaguoti galės tik sistemos administratorius. Ši lenta naudojama tam, kad matyti kokiai įmonei priklauso darbuotojas, meniu ir panašūs dalykai. Todėl laikyti juos enum nėra labai logiška. Šių duomenų pakeitimus gali reikėti atlikti daržniau, todėl kiekvieną kartą perkompiliuoti kodą nėra optimalus variantas.
3. Pridėjome endpoint'us susijusius su `storage` manipuliavimu. Iš pradžių sukuriamas meniu punktas `items` ir tik tuomet galima pridėti ar atimti jų skaičių inventoriuje.
4. Pridėti endpoint'ai susiję su `reservation` manipuliavimu. Juos galima kurti, redaguoti ir panašiai.
5. Pridėjome `giftCards` endpoint'us, kad būtų galima juos kurti, redaguoti ir panašiai.
6. Pridėti endpoint'ai susiję su `establishment` manipuliavimu. Juos galima kurti, redaguoti ir panašiai.
7. Pridėjome `establishmentId`prie `Order` `Item` lentelių. Taip bus galima atskirti kuri įmonė turi tam tikrus item'us ir order'ius.
8. Pridėjome `createdByEmployeeId` prie `Item` lentelės. Taip bus galima atskirti kuris darbuotojas sukūrėt tam tikrus item'us.
9. Pridėti `Service` CRUD endpoint'ai ir frontend funkcionalumas. Galima kurti, redaguoti ir panašiai.