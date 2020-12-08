# Resource - /products

## GET

### Respose

200 Ok

```
{

    "categories": [
        "Liquor",
        "Produce",
        "Dairy",
        "Bread",
        "Meats"
    ],
    "numberOfProductsTota": 8323122,
    "numberOnBackorder": 13,

    "_links": {
        "store-products": {
            "href": "http://localhost:1337/products?category={category}",
            "templated": true
        },
        "store-search": {
            "href": "http://localhost:1337/products?search={term}",
            "templated": true
        },
        "store-details": {
            "href": "http://localhost:1337/products/{id}",
            "templated": "true"
        }
    }
}

````

## POST


# Resource /products/{id}

## GET

200 OK
```
{
    "id": 1,
    "name": "Mystic Momma IPA",
    "category": "Beer",
    "price": 11.99,
    "count": 6
}

```

## DELETE

