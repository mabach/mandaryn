#-*- coding: utf-8 -*- 

# InitalizeCodebook
# Inicjalizuje nową książkę kodową
#
# WEJŚCIE: plik z książką kodową w formacie CSV
# WYJŚCIE: słownik zawierający pary kod, logogram 

def InitalizeCodebook(codebookFile):
    codebook = dict()
    codebookData = open(codebookFile,encoding="utf-8")    
    for line in codebookData:
        temp = line.split(',')
        temp[0] = temp[0].zfill(4)
        temp[1] = temp[1].rstrip()
        codebook[temp[0]] = temp[1]
    return codebook

# SeekByCode
# Zwraca logogram określony podanym kodem
#
# WEJŚCIE: kod, książka kodowa
# WYJŚCIE: logogram

def SeekByCode(code,codebook):
    for k,v in CTC.items():
        if(k == code):
            return v
    return code

# SeekByHanzi
# Zwraca kod podanego logogramu
#
# WEJŚCIE: logogram, książka kodowa
# WYJŚCIE: kod

def SeekByHanzi(hanzi,codebook):
    for k,v in CTC.items():
        if(v == hanzi):
            return k
    return hanzi

# Encode
# Koduje ciąg logogramów na kod telegraficzny
#
# WEJŚCIE: ciąg logogramów,książka kodowa,separator
# WYJŚCIE: ciąg cyfr

def Encode(text,codebook,sep):
    encoded = ''
    for i in range (0, len(text)):
        encoded = encoded + SeekByHanzi(text[i],codebook)+sep
    return encoded

# Decode
# Dekoduje ciąg cyfr na logogramy
# 
# WEJŚĆIE: ciąg cyfr i innych znaków, książka kodowa, separator
# WYJŚCIE: ciąg logogramów

# Inicjalizacja standardowej książki telegraficznej
CTC = InitalizeCodebook("ccc.csv")
print("Wpisz treść telegramu:")
message = input()
print (Encode(message,CTC,' '))
