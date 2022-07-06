# Xmen-WepApi

La funcionalidad del servicio Api se describe a continuación:

URL Web api: https://xmen-webapi.azurewebsites.net

## /Mutant 
Valida si un individuo es mutante de acuerdo a una secuencia de ADN

### Metodo : Post

`Url`

    https://xmen-webapi.azurewebsites.net/Mutant
    
`Body`

    {
      "dna":["ATGCGA","CAGTGC","TTATGT","AGAAGG","CCCCTA","TCACTG"]
    }

### Response

Si el individuo es mutante la respuesta será:

    Status: 200 OK

Si el individuo NO es mutante la respuesta será:

    Status: 403 Forbidden

Si hay un error en el request (no hay secuencia, la matriz es incorrecta, no tienes los caracteres validos, etc...) la respuesta será:

    Status: 400 Bad Request
    
    
## /Stats
Retorna las estadísticas de las verificaciones de ADN registradas

### Metodo : Get

`Url`

    https://xmen-webapi.azurewebsites.net/Stats

### Response

    {
        "count_mutant_dna": 1,
        "count_human_dna": 0
    }
