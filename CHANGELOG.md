1. Pridėjome `items` endpoint'us. Kaip suprantame, *items* yra verslo meniu, ką jie gali pasiūlyti, todėl jiems yra aktualu juos redaguoti, matyti jų kainas ir panašiai.
2. Duomenų modelyje *EstablishmentEnum* keičiame į aktyvią duomenų bazės lentą. Kuriai sukursime ir endpoint'us. Juos redaguoti galės tik sistemos administratorius. Ši lenta naudojama tam, kad matyti kokiai įmonei priklauso darbuotojas, meniu ir panašūs dalykai. Todėl laikyti juos enum nėra labai logiška. Šių duomenų pakeitimus gali reikėti atlikti daržniau, todėl kiekvieną kartą perkompiliuoti kodą nėra optimalus variantas.