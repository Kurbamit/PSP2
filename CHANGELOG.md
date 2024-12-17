Bendrai ant visų `GetAll endpoint'ų` uždėtas paginism. Jo praeita komanda nebuvo numačiusi.
Bendrai pridėjome `GetAll metodus` kiekvienam modeliui, kuris turi *CRUD* operacijas.

1. Pridėjome `items` endpoint'us. Kaip suprantame, `items` yra verslo meniu, ką jie gali pasiūlyti, todėl jiems yra aktualu juos redaguoti, matyti jų kainas ir panašiai.
2. Duomenų modelyje `EstablishmentEnum` keičiame į aktyvią duomenų bazės lentą. Kuriai sukursime ir endpoint'us. Juos redaguoti galės tik sistemos administratorius. Ši lenta naudojama tam, kad matyti kokiai įmonei priklauso darbuotojas, meniu ir panašūs dalykai. Todėl laikyti juos enum nėra labai logiška. Šių duomenų pakeitimus gali reikėti atlikti daržniau, todėl kiekvieną kartą perkompiliuoti kodą nėra optimalus variantas.
3. Pridėjome `giftCards` endpoint'us, kad būtų galima juos kurti, redaguoti ir panašiai. **Praeita komanda nebuvo numačiusi galimybės atsiskaityti su dovanų kuponais.**
4. Pridėti endpoint'ai susiję su `establishment` manipuliavimu. Juos galima kurti, redaguoti ir panašiai. **Praeita komanda nebuvo numačiusi `Establishment` lentelės.**
5. Pridėjome `establishmentId`prie `Order` `Item` lentelių. Taip bus galima atskirti kuri įmonė turi tam tikrus item'us ir order'ius. **Praeita komanda nebuvo numačiusi galimybės sistema naudotis daugiau nei vienai įmonei.**
6. Pridėjome `createdByEmployeeId` prie daugėlio lentelių. Taip bus galima atskirti kuris darbuotojas sukūrėt tam tikrus item'us. **Praeita komanda nebuvo numačiusi galimybės atskirti kas sukūrė tam tikrus duomenis.**
7. Iš `Payment` lentelės į `Order` lentelę perkelėme `Tip`, nes tips turėtų būti visam order, o ne vienam payment.
8.  `Giftcard` lentelės reikšmę `Code` padarėme unikale, kad negalėtų būti dvieju giftcards su tuo pačiu kodu.
9.  Prie `Payment` lentelės pridėjome `Amount`, kad būtų galima žinoti, kiek kiekvienu payment buvo sumokėta.
10. Pridėjome 'payments/createIntent/` endpoint'ą dėl Stripe integracijos. **Praeita komanda nebuvo numačiusi galimybės atsiskaityti su Stripe**
11. Pridėta `Service` lentelė į DB. **Praeita komanda nebuvo numačiusi, kad tokios lentelės reikės. Bet jos reikia norint atlikti rezervaciją. Turi būti galima kažkokiu būdu peržiūrėti laisvus laikus. `Service` lentelė pagrinde tam ir yra skirta. Yra skirtingų paslaugų, kurios gali trukti skirtingą kiekį laiko.**
12. Pridėti `Service` CRUD endpoint'ai ir frontend funkcionalumas. Galima kurti, redaguoti ir panašiai.
13. Pridėta `WorkingHours` lentelė į DB. **Praeita komanda nebuvo numačiusi kokiu būdu bus apskaičiuojamas laikas rezervacijoms atlikti. Reikia kažkur saugoti verslo darbo valandas, kad žinot nuo kada, iki kada galima atlikti rezervacijas.**
14. Pridėti `WorkingHours` CRUD endpoint'ai kurimui, redagavimui ir panašiai.
15. `Reservation` DB lentelės atnaujinimai, kad laikytų koks servisas atliekamas.
16. Į `FullOrder` lentelę pridėjome `item` duomenų saugojimą. Tam, kad būtų galima turėti istorinius duomenis.