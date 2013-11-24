# -*- coding: utf-8 -*
import os
import shutil

# Kod obsługi telegrafii

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

def GenerateNewName(entry,level):
        newName = ''
        Hanzi = SeekByHanzi(entry[0],CTC)
        
        filename = (Hanzi+"-"+level+".png")
        return filename

def OrderHanzi():
        HZlist = []
        for fn in os.listdir(path):
                if (os.path.isfile(fn)):
                        if("ccc.csv"   in fn):
                                pass
                        if("py" in fn):
                                pass
                        else:
                                HanziString = fn
                                baselist = HanziString.split('.')
                                if (baselist[0] == "ccc"):
                                        continue
                                name     =   ( GenerateNewName(baselist[0],baselist[1]) )
                                shutil.copyfile(fn,name)
                                
        pass

#Initalization of standard telegraph codebook
CTC = InitalizeCodebook("ccc.csv")
print('Koder telegraficzny gotowy')
path = u'.'


        
print("Przygotowanie bazy danych znaków")
OrderHanzi()
print("Gotowe")
