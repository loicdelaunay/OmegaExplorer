<template>
  <v-container fluid>
    <v-card class="hero-card rounded-xl">
      <v-row align="stretch" justify="stretch">
        <!-- left column for logo -->
        <v-col cols="6">
          <div class="ma-12">
            <!-- Title -->
            <div>
              <v-row align="center" justify="center">
                <h1 class="header-title">Omega</h1>
                <h2 class="header-title-2">Explorer</h2>
              </v-row>
            </div>

            <!-- Subtitle -->
            <p class="header-subtitle">Explore the Omega-3 Fatty Acids</p>

            <!-- Button play with free description -->
          </div>
        </v-col>

        <!-- right column for images -->
        <v-col cols="6">
          <div class="ma-12">
            <img width="250px" src="@/assets/spaceship.png" alt="Omega Explorer Logo" class="header-logo">
          </div>
        </v-col>
      </v-row>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import './Header.css'
</script>
