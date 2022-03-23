# CashRegister
 C# crossplateform cash register app for small companies

## Objectifs principaux

- Multiplateforme (iOS, Android, Windows)
- Utilisation d'un serveur Azure servant de SGBD MySQL
- Gestion des scans d'articles (portable & webcam)
- Envois des tickets de caisse à une adresse mail
- Gestion automatique des devises

### 3 modes

- Mode caissier: ajouter articles d'un client et le faire payer
    - scanner un article / ajouter à la main (un ou plusieurs)
	- supprimer article
	- voir les articles en cours (nom, description, quantite, prix total, prix unitaire)
	- payement (twint)
	- ajout de nouveaux articles dans la db grâce au code barre depuis une api (ex. openfoodfacts)
- Mode client: ajouter soi-même les articles et payer en caisse
    - Comme le passabene du magasin Suisse "Coop" (aussi connu sous le nom de Coopé, ou Là Coopéy)
	- scan final à un terminal
- Mode administration: modification du stock
    - Ajout de produit
        - Ajout de nouveaux produits
        - Quantité disponible
    - Gestion des stocks
    - Contrôle d'inventaire
        - Effectuer l'inventaire en listant tous les articles physiquement présents dans le stock comparé à ceux qui devraient y être d'après la base de donnée
    - Statistiques de vente

## Répartition des objectifs

BDD / Serveur Azure
ajout/suppression/visualisation articles dans le panier
scan code barre
ajout de nouveaux articles avec API
génération de tickets de caisse & envoi par mail
payement
mode admin
gestion des devises