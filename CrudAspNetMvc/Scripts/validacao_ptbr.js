//Botão direito na pasta "Scripts" > Add > New Item > Javascript File

/*
extende o método date do jquery validator (objeto do plugin validate responsável por fazer as validações)
jquery validator tem um conjunto de métodos que são usados para determinados tipos de dados
a função date lê o valor que está no elemento input e valida usando expressão regular (regex), para usar o formato brasileiro dia/mês/ano
*/
jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    }
});