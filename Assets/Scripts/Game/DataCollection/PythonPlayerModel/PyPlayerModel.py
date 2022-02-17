import UnityEngine
import glob
import pandas as pd
from sklearn.preprocessing import StandardScaler
from sklearn.preprocessing import LabelBinarizer
import pickle

# This is the C# System.dll not the Python sys module.
import System

import UnityEngine
import UnityEditor

#Leitura de todos os arquivos de level
files_level = glob.glob("./*/*Level.csv")
linhas_level = []
#Adicionar cabeçalho personalizado
linhas_level.append('player_id,map_id,chosen_weapon,elapsed_time,has_finished,has_died,total_keys,collected_keys,total_locks,opened_locks,total_rooms,number_of_visited_rooms,total_visits,number_of_over_visited_rooms,player_initial_health,player_final_health,player_lost_health,number_of_enemies,number_of_killed_enemies,number_of_npcs,number_of_interacted_npcs,total_treasures,collected_treasures,max_combo,PostQuestion 0,PostQuestion 1,PostQuestion 2,PostQuestion 3,PostQuestion 4,PostQuestion 5,PostQuestion 6,PostQuestion 7,PostQuestion 8,PostQuestion 9,PostQuestion 10,PostQuestion 11,Arq')

for f in files_level:
    arq = open(f,'r')
    for l in arq:
        #Pular cabeçalho original
        if 'player_id' in l:
            print('cabeçalho')
        #Pular linhas vazias
        elif l=='\n':
            print('linha vazia')
        #Escrever somente linhas de dados
        else:
            #Adicionando o nome do arquivo ao último campo Arq para identificação
            l = l.replace('\n', f.split('\\')[2]).split(',')
            linhas_level.append(l)
linhas_level

#Leitura de todos os arquivos de player
files_player = glob.glob("./*/*Player.csv")
linhas_player = []
#Adicionar cabeçalho personalizado
linhas_player.append('Profile,ExperimentalProfile,PreQuestion 0,PreQuestion 1,PreQuestion 2,PreQuestion 3,PreQuestion 4,PreQuestion 5,PreQuestion 6,PreQuestion 7,PreQuestion 8,PreQuestion 9,PreQuestion 10,PreQuestion 11,Arq')

for f in files_player:
    arq = open(f,'r')
    for l in arq:
        #Pular cabeçalho original
        if 'Profile' in l:
            print('cabeçalho')
        #Pular linhas vazias
        elif l=='\n':
            print('linha vazia')
        #Escrever somente linhas de dados
        else:
            #Adicionando o nome do arquivo ao último campo Arq para identificação
            l = l.replace('\n', f.split('\\')[2]).split(',')
            linhas_player.append(l)

import pandas as pd
import numpy as np

Level = pd.DataFrame(linhas_level[1:], columns=linhas_level[0].split(','))

Player = pd.DataFrame(linhas_player[1:], columns=linhas_player[0].split(','))

#Tratamento pré-teste
preTeste = Player.copy()
preTeste['ID'] = preTeste['Arq'].str.slice(start=0, stop=-11) #Criar o ID para cruzamento de dados a partir do nome do arquivo
for i in preTeste.columns:
    preTeste.loc[preTeste[i] == '-1', i] = np.nan #Os dados -1 são referentes a respostas não respondidas por problemas
preTeste = preTeste.drop_duplicates() #Todo gameplay repetiu os dados, por isso a remoção de duplicados
#Depois de deletar os números -1, deletar todas as linhas que são sem valores
preTeste = preTeste.dropna(axis=0, subset=['PreQuestion 0', 'PreQuestion 1', 'PreQuestion 2', 'PreQuestion 3', 'PreQuestion 4', 'PreQuestion 5', 'PreQuestion 6', 'PreQuestion 7', 'PreQuestion 8', 'PreQuestion 9', 'PreQuestion 10', 'PreQuestion 11'])
preTeste = preTeste.drop(columns=['Arq'])

#Tratamento gameplay
gameplay = Level[['has_finished', 'has_died', 'total_keys', 'collected_keys', 'total_locks', 'opened_locks', 'total_rooms', 'number_of_visited_rooms', 'total_visits', 'player_lost_health','number_of_enemies', 'number_of_killed_enemies', 'number_of_npcs', 'number_of_interacted_npcs', 'total_treasures', 'collected_treasures', 'max_combo', 'Arq']]

#Transformando os campos boolean em 0 e 1
gameplay["has_finished"] = gameplay["has_finished"].astype(bool).astype(int)
gameplay["has_died"] = gameplay["has_died"].astype(bool).astype(int)

#Criando os campos calculados, como estipulado no trabalho
gameplay['rooms_visited'] = (gameplay['total_visits'].astype(float)/gameplay['total_rooms'].astype(float)).round(2)
gameplay['unique_rooms_conclusion'] = (gameplay['number_of_visited_rooms'].astype(float)/gameplay['total_rooms'].astype(float)).round(2)
gameplay['keys_found'] = (gameplay['collected_keys'].astype(float)/gameplay['total_keys'].astype(float)).round(2)
gameplay['doors_unlocked'] = (gameplay['opened_locks'].astype(float)/gameplay['total_locks'].astype(float)).round(2)
gameplay['treasures_foud'] = (gameplay['collected_treasures'].astype(float)/gameplay['total_treasures'].astype(float)).round(2)
gameplay['enemies_fought'] = (gameplay['number_of_killed_enemies'].astype(float)/gameplay['number_of_enemies'].astype(float)).round(2)

#Gerando o campo ID e tirando as colunas inválidas
gameplay['ID'] = gameplay['Arq'].str.slice(start=0, stop=-10)
gameplay = gameplay.drop(columns=['Arq', 'total_keys', 'collected_keys', 'total_locks', 'opened_locks', 'total_rooms', 'number_of_visited_rooms', 'total_visits', 'number_of_enemies', 'number_of_killed_enemies', 'number_of_npcs', 'number_of_interacted_npcs', 'total_treasures', 'collected_treasures'])

gameplay
#Tratamento pós-teste
posTeste = Level[['PostQuestion 0', 'PostQuestion 1', 'PostQuestion 2', 'PostQuestion 3', 'PostQuestion 4', 'PostQuestion 5', 'PostQuestion 6', 'PostQuestion 7', 'PostQuestion 8', 'PostQuestion 9', 'PostQuestion 10', 'PostQuestion 11', 'Arq']]
for i in posTeste.columns:
    posTeste.loc[posTeste[i] == '-1', i] = np.nan
posTeste = posTeste.dropna(axis=0, subset=['PostQuestion 0', 'PostQuestion 1', 'PostQuestion 2', 'PostQuestion 3', 'PostQuestion 4', 'PostQuestion 5', 'PostQuestion 6', 'PostQuestion 7'])
posTeste['ID'] = posTeste['Arq'].str.slice(start=0, stop=-10)
posTeste

#Realizando um inner join para garantir que não há linhas de gameplay sem pré-teste e/ou pós-teste
comb = gameplay.merge(preTeste, on='ID', how='inner').merge(posTeste, on='ID', how='inner')

#seleção de features
features = comb[['has_finished', 'has_died', 'player_lost_health', 'max_combo', 'rooms_visited', 'unique_rooms_conclusion', 'keys_found', 'doors_unlocked', 'treasures_foud', 'enemies_fought', 'PreQuestion 0', 'PreQuestion 1', 'PreQuestion 2', 'PreQuestion 3', 'PreQuestion 4', 'PreQuestion 5', 'PreQuestion 6', 'PreQuestion 7', 'PreQuestion 8', 'PreQuestion 9', 'PreQuestion 10', 'PreQuestion 11']]
#separação das features numéricas
features_num = features[['player_lost_health', 'max_combo', 'rooms_visited', 'unique_rooms_conclusion', 'keys_found', 'doors_unlocked', 'treasures_foud', 'enemies_fought']]

#normalização desses valores numéricos
features_norm = pd.DataFrame(StandardScaler().fit_transform(features_num))

#Geração dos valores de pré-teste como one-hot-encoding

q0_a = np.array(features['PreQuestion 0'])
q0 = pd.DataFrame(LabelBinarizer().fit_transform(q0_a))

q1_a = np.array(features['PreQuestion 1'])
q1 = pd.DataFrame(LabelBinarizer().fit_transform(q1_a))

q2_a = np.array(features['PreQuestion 2'])
q2 = pd.DataFrame(LabelBinarizer().fit_transform(q2_a))

q3_a = np.array(features['PreQuestion 3'])
q3 = pd.DataFrame(LabelBinarizer().fit_transform(q3_a))

q4_a = np.array(features['PreQuestion 4'])
q4 = pd.DataFrame(LabelBinarizer().fit_transform(q4_a))

q5_a = np.array(features['PreQuestion 5'])
q5 = pd.DataFrame(LabelBinarizer().fit_transform(q5_a))

q5_a = np.array(features['PreQuestion 5'])
q5 = pd.DataFrame(LabelBinarizer().fit_transform(q5_a))

q6_a = np.array(features['PreQuestion 6'])
q6 = pd.DataFrame(LabelBinarizer().fit_transform(q6_a))

q7_a = np.array(features['PreQuestion 7'])
q7 = pd.DataFrame(LabelBinarizer().fit_transform(q7_a))

q8_a = np.array(features['PreQuestion 8'])
q8 = pd.DataFrame(LabelBinarizer().fit_transform(q8_a))

q9_a = np.array(features['PreQuestion 9'])
q9 = pd.DataFrame(LabelBinarizer().fit_transform(q9_a))

q10_a = np.array(features['PreQuestion 10'])
q10 = pd.DataFrame(LabelBinarizer().fit_transform(q10_a))

q11_a = np.array(features['PreQuestion 11'])
q11 = pd.DataFrame(LabelBinarizer().fit_transform(q11_a))

#Adição dos valores de has_finished e has_died
features_norm['8']=features['has_finished']
features_norm['9']=features['has_died']

#Adição das colunas de pré-teste com onehotencoding
for i in range(len(features['PreQuestion 0'].unique())):
    nome = 'q_0_'+str(i+1)
    features_norm[nome] = q0[i]

for i in range(len(features['PreQuestion 1'].unique())):
    nome = 'q_1_'+str(i+1)
    features_norm[nome] = q1[i]

for i in range(len(features['PreQuestion 2'].unique())):
    nome = 'q_2_'+str(i+1)
    features_norm[nome] = q2[i]
    
for i in range(len(features['PreQuestion 3'].unique())):
    nome = 'q_3_'+str(i+1)
    features_norm[nome] = q3[i]
    
for i in range(len(features['PreQuestion 4'].unique())):
    nome = 'q_4_'+str(i+1)
    features_norm[nome] = q4[i]
    
for i in range(len(features['PreQuestion 5'].unique())):
    nome = 'q_5_'+str(i+1)
    features_norm[nome] = q5[i]
    
for i in range(len(features['PreQuestion 6'].unique())):
    nome = 'q_6_'+str(i+1)
    features_norm[nome] = q6[i]
    
for i in range(len(features['PreQuestion 7'].unique())):
    nome = 'q_7_'+str(i+1)
    features_norm[nome] = q7[i]
    
for i in range(len(features['PreQuestion 8'].unique())):
    nome = 'q_8_'+str(i+1)
    features_norm[nome] = q8[i]
    
for i in range(len(features['PreQuestion 9'].unique())):
    nome = 'q_9_'+str(i+1)
    features_norm[nome] = q9[i]
    
for i in range(len(features['PreQuestion 10'].unique())):
    nome = 'q_10_'+str(i+1)
    features_norm[nome] = q10[i]
    
for i in range(len(features['PreQuestion 11'].unique())):
    nome = 'q_11_'+str(i+1)
    features_norm[nome] = q11[i]

pd.set_option('display.max_columns', 500)
pd.set_option('display.max_rows', 500)
features_norm.dtypes

#Seleção de rótulos
yq2 = comb['PostQuestion 2'].astype('float')
yq3 = comb['PostQuestion 3'].astype('float')
yq5 = comb['PostQuestion 5'].astype('float')
yq6 = comb['PostQuestion 6'].astype('float')
yq7 = comb['PostQuestion 7'].astype('float')

modelQ2 = pickle.load(open('modeloQ2.sav', 'rb'))
result = modelQ2.score(features_norm, yq2)
print(result)

modelQ3 = pickle.load(open('modeloQ3.sav', 'rb'))
result = modelQ3.score(features_norm, yq3)
print(result)

modelQ5 = pickle.load(open('modeloQ5.sav', 'rb'))
result = modelQ5.score(features_norm, yq5)
print(result)

modelQ6 = pickle.load(open('modeloQ6.sav', 'rb'))
result = modelQ6.score(features_norm, yq6)
print(result)

modelQ7 = pickle.load(open('modeloQ7.sav', 'rb'))
result = modelQ7.score(features_norm, yq7)
print(result)

modelQ7.predict(features_norm[0])