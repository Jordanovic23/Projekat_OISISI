# Sajam knjiga

Konzolna aplikacija za upravljanje sajmom knjiga (spec. tačke 8.1–8.4):
dodavanje, izmena, brisanje i pregled svih entiteta, uz čuvanje u CSV datoteke.

## Autori
- Student 1: Bogdan Rakić
- Student 2: Aleksa Antić

## Podela posla

**Bogdan Rakić**
- Modeli: Posetilac, Kupovina, Izdavač
- DAO: PosetilacDAO, IzdavacDAO, AutorKnjigaDAO; dopune KupovinaDAO i ListaZeljaDAO
- Posetioci: prikaz, dodavanje, izmena, brisanje
- Knjiga: dodavanje i izmena

**Aleksa Antić**
- Modeli: Autor, Knjiga, Adresa
- Autori: prikaz, dodavanje, izmena, brisanje
- Knjiga: prikaz i brisanje
- Servisi (Core/Services) i konzolni klijent (ConsoleClient) za sve entitete

## Struktura
- `Core`: modeli, DAO klase, servisi i čuvanje u CSV (`Storage`)
- `ConsoleClient`: kontroleri i meniji konzolne aplikacije
- `Data`: CSV datoteke sa podacima

## Pokretanje
Otvoriti `SajamKnjiga.slnx` u Visual Studiju i pokrenuti projekat `ConsoleClient`.
Podaci se čuvaju u folderu `Data` pored solution fajla.
